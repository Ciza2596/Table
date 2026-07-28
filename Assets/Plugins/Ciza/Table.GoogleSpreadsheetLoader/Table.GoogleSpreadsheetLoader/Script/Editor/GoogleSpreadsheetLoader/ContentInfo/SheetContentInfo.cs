using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CizaTable.Editor
{
	[Serializable]
	public class SheetContentInfo
	{
		[SerializeField]
		private SheetContent _sheetContent;

		[HideInInspector]
		[SerializeField]
		private string _sheetInfoId;

		[HideInInspector]
		[SerializeField]
		private string _spreadSheetId;

		[HideInInspector]
		[SerializeField]
		private string _sheetId;

#if UNITY_EDITOR
		[HideInInspector]
		[SerializeField]
		private bool _isBusy;
#endif

		public bool IsRemoved => _sheetContent == null;

		//constructor
		public SheetContentInfo(string sheetInfoId, string spreadSheetId, string sheetId, SheetContent sheetContent)
		{
			_sheetInfoId = sheetInfoId;

			_spreadSheetId = spreadSheetId;
			_sheetId = sheetId;

			_sheetContent = sheetContent;
		}

		//public variable
		public string SheetInfoId => _sheetInfoId;

		public string SpreadSheetId => _spreadSheetId;
		public string SheetId => _sheetId;

		//public method
		public void SetIsBusy(bool isBusy) =>
			_isBusy = isBusy;

		public void Update(string sheetName, string folderPath, string csv)
		{
			_sheetContent.name = sheetName;
			var currentAssetPath = AssetDatabase.GetAssetPath(_sheetContent);

			var assetPath = PathHelper.GetFullPath(folderPath, sheetName);

			if (currentAssetPath != assetPath)
			{
				if (!Directory.Exists(folderPath))
					Directory.CreateDirectory(folderPath);

				AssetDatabase.MoveAsset(currentAssetPath, assetPath);
			}

			CreateDataUnitsAndRawData(csv, out var dataMapList, out var rawData);
			_sheetContent.UpdateContent(csv, dataMapList, rawData);
			EditorUtility.SetDirty(_sheetContent);
		}

		public void Remove()
		{
			var subSheetContent = _sheetContent;
			_sheetContent = null;

			var assetPath = AssetDatabase.GetAssetPath(subSheetContent);
			AssetDatabase.DeleteAsset(assetPath);
			AssetDatabase.SaveAssets();
			Debug.Log($"[SubSheetContentInfo::Remove] Remove content file : {assetPath}.");
		}

		private void CreateDataUnitsAndRawData(string csv, out DataMapList dataMapList, out string[,] rawData)
		{
			//讀入 CSV 檔案，使其分為 string 二維陣列
			var csvTable = CsvParserUtils.Parse(csv);

			dataMapList = new DataMapList();
			var labels = new List<string>();
			int usedLength = 0;
			for (var i = 0; i < csvTable[0].Length; i++)
			{
				var key = csvTable[0][i];
				if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
					break;

				usedLength = i + 1;
				labels.Add(key);
			}

			for (var i = 1; i < csvTable.Length; i++)
			{
				var dataValues = new List<DataValue>();

				for (var j = 1; j < usedLength; j++)
				{
					var name = labels[j];
					var value = j < csvTable[i].Length ? csvTable[i][j] : string.Empty;

					var dataValue = new DataValue(name, value);
					dataValues.Add(dataValue);
				}

				var key = csvTable[i][0];

				//var dataUnit = new DataUnit(key, dataValues.ToArray());
				dataMapList.Add(key, dataValues.ToArray());
			}

			//Read Raw Data
			rawData = new string[usedLength, csvTable.Length];
			for (var i = 0; i < csvTable.Length; i++)
				for (var j = 0; j < usedLength; j++)
					rawData[j, i] = j < csvTable[i].Length ? csvTable[i][j] : string.Empty;
		}
	}
}
