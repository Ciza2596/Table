using System.Collections.Generic;
using System.Threading.Tasks;
using CizaTable;
using UnityEditor;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
	[CreateAssetMenu(fileName = "GoogleSpreadsheetLoader", menuName = "Ciza/Table/GoogleSpreadsheetLoader")]
	public class GoogleSpreadsheetLoader : ScriptableObject, IZomeraphyPanel
	{
		//private variable
		[SerializeField]
		private string _webAppUrl;

		[SerializeField]
		private List<SpreadsheetInfo> _spreadsheetInfos;

		[SerializeField]
		private List<SpreadsheetContentInfo> _usedSpreadsheetContentInfos = new List<SpreadsheetContentInfo>();

		private bool                        _isBusy;
		private GoogleSpreadsheetGasHandler _googleSpreadsheetGasHandler = new GoogleSpreadsheetGasHandler();

		//public method
		public virtual async Task UpdateSheetContentInfo(SheetContentInfo sheetContentInfo)
		{
			sheetContentInfo.SetIsBusy(true);

			var spreadSheetId = sheetContentInfo.SpreadSheetId;
			var sheetId       = sheetContentInfo.SheetId;

			var spreadsheetName = await _googleSpreadsheetGasHandler.GetSpreadsheetName(_webAppUrl, spreadSheetId);
			var sheetName       = await _googleSpreadsheetGasHandler.GetSheetName(_webAppUrl, spreadSheetId, sheetId);

			var spreadsheetInfo = _spreadsheetInfos.Find(spreadsheetInfo => spreadsheetInfo.SpreadsheetId == spreadSheetId);

			var sheetContentPath = spreadsheetInfo.SheetContentPath;
			var folderPath       = PathHelper.GetFolderPath(sheetContentPath, spreadsheetName);

			var csv = await _googleSpreadsheetGasHandler.GetGoogleSheetCsv(_webAppUrl, spreadSheetId, sheetId);

			sheetContentInfo.Update(sheetName, folderPath, csv);
			sheetContentInfo.SetIsBusy(false);
		}

		public virtual void CheckIsSheetContentRemoved()
		{
			foreach (var spreadsheetContentInfo in _usedSpreadsheetContentInfos)
				spreadsheetContentInfo.CheckIsSheetContentRemoved();
		}


		[ContextMenu("Update All Spreadsheets")]
		public async Task UpdateSpreadsheets()
		{
			if (_spreadsheetInfos is null || _spreadsheetInfos.Count <= 0)
				return;

			Debug.Log("[GoogleSpreadsheetLoader::UpdateSpreadsheets] Start update spreadsheets....");

			foreach (var spreadsheetInfo in _spreadsheetInfos)
			{
				var spreadsheetId = spreadsheetInfo.SpreadsheetId;
				if (string.IsNullOrEmpty(spreadsheetId))
					continue;

				var spreadSheetName = await _googleSpreadsheetGasHandler.GetSpreadsheetName(_webAppUrl, spreadsheetId);
				spreadsheetInfo.SetSpreadSheetName(spreadSheetName);

				var googleSheetInfos = await _googleSpreadsheetGasHandler.GetGoogleSheetInfos(_webAppUrl, spreadsheetId);
				foreach (var googleSheetInfo in googleSheetInfos)
				{
					var sheetName = googleSheetInfo.SheetName;
					var sheetId   = googleSheetInfo.SheetId;

					var sheetInfo = spreadsheetInfo.FindSheetInfo(sheetId);

					if (sheetInfo is null)
						sheetInfo = spreadsheetInfo.CreateSheetInfo(sheetId);

					if (sheetInfo.Name != sheetName)
						sheetInfo.SetName(sheetName);
				}

				var removeCount = spreadsheetInfo.SheetInfos.Count - googleSheetInfos.Length;
				if(removeCount > 0)
					spreadsheetInfo.RemoveSheetInfo(removeCount);

				spreadsheetInfo.OrderByIsUsing();
			}

			Debug.Log("[GoogleSpreadsheetLoader::UpdateSpreadsheets] Spreadsheets is updated.");
		}

		[ContextMenu("Update All Spreadsheet Contents")]
		public async Task UpdateAllUsedSheetContentInfos()
		{
			if (_isBusy)
				return;

			Debug.Log("[GoogleSpreadsheetLoader::UpdateAllUsedSheetContentInfos] Start update all used sheet contents....");

			_isBusy = true;

			CheckIsSheetContentRemoved();

			var sheetContentInfoUpdates = new List<Task>();
			foreach (var spreadsheetInfo in _spreadsheetInfos)
			{
				var spreadsheetInfoId      = spreadsheetInfo.GetId();
				var spreadsheetContentInfo = FindUsedSpreadSheetContentInfo(spreadsheetInfoId);

				if (spreadsheetContentInfo is null)
					spreadsheetContentInfo = CreateUsedSpreadSheetContentInfo(spreadsheetInfoId);

				var sheetContentPath = spreadsheetInfo.SheetContentPath;

				if (sheetContentPath != spreadsheetContentInfo.SheetContentPath)
					spreadsheetContentInfo.SetSheetContentPath(sheetContentPath);

				var spreadsheetId   = spreadsheetInfo.SpreadsheetId;
				var spreadSheetName = spreadsheetInfo.SpreadsheetName;

				foreach (var sheetInfo in spreadsheetInfo.SheetInfos)
				{
					var sheetInfoId = sheetInfo.Id;
					var sheetId     = sheetInfo.SheetId;

					var sheetContentInfo = spreadsheetContentInfo.FindSheetContentInfo(sheetInfoId);
					if (sheetInfo.IsUsing)
					{
						if (sheetContentInfo is null)
							sheetContentInfo = spreadsheetContentInfo.CreateSheetContentInfo(sheetInfoId, spreadsheetId, sheetId, spreadSheetName, this);

						sheetContentInfoUpdates.Add(UpdateSheetContentInfo(sheetContentInfo));
						continue;
					}

					if (sheetContentInfo != null)
						sheetContentInfo.Remove();
				}
			}

			await Task.WhenAll(sheetContentInfoUpdates);

			EditorUtility.SetDirty(this);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			_isBusy = false;

			Debug.Log("[GoogleSpreadsheetLoader::UpdateAllUsedSheetContentInfos] update all used sheet contents is updated.");
		}

		private void RemoveAllUsedSheetContentInfos()
		{
			var sheetContentInfos = _usedSpreadsheetContentInfos.ToArray();
			_usedSpreadsheetContentInfos.Clear();

			foreach (var sheetContentInfo in sheetContentInfos)
				sheetContentInfo.RemoveAll();
		}

		private void ResetBusy()
		{
			var sheetContentInfos = _usedSpreadsheetContentInfos.ToArray();
			foreach (var sheetContentInfo in sheetContentInfos)
				foreach (var subSheetContentInfo in sheetContentInfo.SheetContentInfos)
					subSheetContentInfo.SetIsBusy(false);

			_isBusy = false;
		}

		private SpreadsheetContentInfo CreateUsedSpreadSheetContentInfo(string spreadsheetInfoId)
		{
			var spreadsheetContentInfo = new SpreadsheetContentInfo(spreadsheetInfoId);
			_usedSpreadsheetContentInfos.Add(spreadsheetContentInfo);
			return spreadsheetContentInfo;
		}

		private SpreadsheetContentInfo FindUsedSpreadSheetContentInfo(string spreadsheetInfoId) =>
			_usedSpreadsheetContentInfos.Find(spreadsheetContentInfo => spreadsheetContentInfo.SpreadsheetInfoId == spreadsheetInfoId);
	}
}
