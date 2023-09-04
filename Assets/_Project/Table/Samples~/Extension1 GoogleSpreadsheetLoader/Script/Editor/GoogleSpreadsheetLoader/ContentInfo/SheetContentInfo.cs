using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
	[Serializable]
	public class SheetContentInfo
	{
		//private variable
		[TableColumnWidth(200)]
		[VerticalGroup("ScriptableObject")]
		[ReadOnly]
		[SerializeField]
		private SheetContent _sheetContent;

		[HideInInspector]
		[SerializeField]
		private GoogleSpreadsheetLoader _googleSpreadsheetLoader;

		[HideInInspector]
		[SerializeField]
		private string _sheetInfoId;

		[HideInInspector]
		[SerializeField]
		private string _spreadSheetId;

		[HideInInspector]
		[SerializeField]
		private string _sheetId;

		private bool _isBusy;

		public bool IsRemoved => _sheetContent == null;

		//constructor
		public SheetContentInfo(string sheetInfoId, string spreadSheetId, string sheetId, SheetContent sheetContent, GoogleSpreadsheetLoader googleSpreadsheetLoader)
		{
			_sheetInfoId = sheetInfoId;

			_spreadSheetId = spreadSheetId;
			_sheetId       = sheetId;

			_sheetContent            = sheetContent;
			_googleSpreadsheetLoader = googleSpreadsheetLoader;
		}

		//public variable
		public string SheetInfoId   => _sheetInfoId;
		public string SpreadSheetId => _spreadSheetId;
		public string SheetId       => _sheetId;

		//public method
		public void SetIsBusy(bool isBusy) =>
			_isBusy = isBusy;

		public void Update(string sheetName, string folderPath, string csv)
		{
			_sheetContent.name = sheetName;
			var instanceId = _sheetContent.GetInstanceID();
			AssetDatabase.TryGetGUIDAndLocalFileIdentifier(instanceId, out string guid, out long localId);
			var currentAssetPath = AssetDatabase.GUIDToAssetPath(guid);

			var assetPath = PathHelper.GetFullPath(folderPath, sheetName);

			if (currentAssetPath != assetPath)
			{
				if (!Directory.Exists(folderPath))
					Directory.CreateDirectory(folderPath);

				AssetDatabase.MoveAsset(currentAssetPath, assetPath);
			}

			CreateDataUnitsAndRawData(csv, out var dataUnits, out var rawData);
			_sheetContent.UpdateContent(dataUnits.ToArray(), rawData);
			EditorUtility.SetDirty(_sheetContent);
		}

		[HorizontalGroup("動作")]
		[GUIColor(1, 0, 0)]
		[Button("移除")]
		[DisableIf("_isBusy")]
		public void Remove()
		{
			var subSheetContent = _sheetContent;
			_sheetContent = null;

			var assetPath = AssetDatabase.GetAssetPath(subSheetContent);
			AssetDatabase.DeleteAsset(assetPath);
			AssetDatabase.SaveAssets();
			Debug.Log($"[SubSheetContentInfo::Remove] Remove content file : {assetPath}.");
		}

		//private method
		[HorizontalGroup("動作")]
		[Button("更新")]
		[GUIColor(0, 1, 0)]
		[DisableIf("_isBusy")]
		private async void Update()
		{
			try
			{
				await _googleSpreadsheetLoader.UpdateSheetContentInfo(this);
			}
			catch
			{
				_isBusy = false;
			}
		}

		private void CreateDataUnitsAndRawData(string csv, out List<DataUnit> dataUnits, out string[,] rawData)
		{
			//讀入 CSV 檔案，使其分為 string 二維陣列
			var csvParser = new CsvParser.CsvParser();
			var csvTable  = csvParser.Parse(csv);

			dataUnits = new List<DataUnit>();
			var labels     = new List<string>();
			var usedLength = csvTable[0].Length;
			for (var i = 0; i < csvTable[0].Length; i++)
			{
				var key = csvTable[0][i];
				if (string.IsNullOrWhiteSpace(key))
				{
					usedLength = i;
					break;
				}

				labels.Add(key);
			}

			for (var i = 1; i < csvTable.Length; i++)
			{
				var dataValues = new List<DataValue>();

				for (var j = 1; j < usedLength; j++)
				{
					var name  = labels[j];
					var value = csvTable[i][j];

					var dataValue = new DataValue(name, value);
					dataValues.Add(dataValue);
				}

				var key = csvTable[i][0];

				var dataUnit = new DataUnit(key, dataValues.ToArray());
				dataUnits.Add(dataUnit);
			}

			//Read Raw Data
			rawData = new string[usedLength, csvTable.Length];
			for (var i = 0; i < csvTable.Length; i++)
				for (var j = 0; j < usedLength; j++)
					rawData[j, i] = csvTable[i][j];
		}
	}
}
