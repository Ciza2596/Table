using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CizaTable.Editor
{
	[CreateAssetMenu(fileName = "Tbl.GoogleSpreadsheetLoader.asset", menuName = "Ciza/Table/GoogleSpreadsheetLoader")]
	public class GoogleSpreadsheetLoader : ScriptableObject, IZomeraphyPanel
	{
		//private variable
		[SerializeField]
		private string _webAppUrl;

		[SerializeField]
		private List<SpreadsheetInfo> _spreadsheetInfos;

		[SerializeField]
		private List<SpreadsheetContentInfo> _usedSpreadsheetContentInfos = new List<SpreadsheetContentInfo>();

		[HideInInspector]
		[SerializeField]
		private bool _isBusy;

		private readonly GoogleSpreadsheetGasHandler _googleSpreadsheetGasHandler = new GoogleSpreadsheetGasHandler();

		//public method
		public virtual async Task UpdateSheetContentInfo(SheetContentInfo sheetContentInfo)
		{
			var spreadSheetId = sheetContentInfo.SpreadSheetId;
			var sheetId = sheetContentInfo.SheetId;

			var spreadsheetInfo = _spreadsheetInfos.FirstOrDefault(spreadsheetInfo => spreadsheetInfo.SpreadsheetId == spreadSheetId);
			if (spreadsheetInfo == null)
				return;

			var sheetInfo = spreadsheetInfo.FindSheetInfo(sheetId);
			if (sheetInfo == null)
				return;

			Debug.Log($"[GoogleSpreadsheetLoader::UpdateSheetContentInfo] {sheetInfo.FullName} update start.");
			sheetContentInfo.SetIsBusy(true);

			var sheetContentPath = spreadsheetInfo.SheetContentPath;
			var folderPath = PathHelper.GetFolderPath(sheetContentPath, sheetInfo.SpreadsheetName);

			var csv = await _googleSpreadsheetGasHandler.GetGoogleSheetCsv(_webAppUrl, spreadSheetId, sheetId);

			sheetContentInfo.Update(sheetInfo.SheetName, folderPath, csv);
			sheetContentInfo.SetIsBusy(false);
			Debug.Log($"[GoogleSpreadsheetLoader::UpdateSheetContentInfo] {sheetInfo.FullName} update end.");
		}

		public virtual void CheckIsSheetContentRemoved()
		{
			foreach (var spreadsheetContentInfo in _usedSpreadsheetContentInfos)
				spreadsheetContentInfo.CheckIsSheetContentRemoved();
		}


		[ContextMenu("Update Spreadsheet Preview")]
		public async Task UpdateSpreadsheetPreview()
		{
			if (_spreadsheetInfos is null || _spreadsheetInfos.Count <= 0)
				return;

			Debug.Log($"[GoogleSpreadsheetLoader::UpdateSpreadsheetPreview] Spreadsheets update start. ===================================");
			foreach (var spreadsheetInfo in _spreadsheetInfos)
			{
				var spreadsheetId = spreadsheetInfo.SpreadsheetId;
				if (string.IsNullOrEmpty(spreadsheetId))
					continue;

				var spreadsheetName = await _googleSpreadsheetGasHandler.GetSpreadsheetName(_webAppUrl, spreadsheetId);
				Debug.Log($"[GoogleSpreadsheetLoader::UpdateSpreadsheetPreview] {spreadsheetName} update start.");
				spreadsheetInfo.SetSpreadSheetName(spreadsheetName);

				var googleSheetInfos = await _googleSpreadsheetGasHandler.GetGoogleSheetInfos(_webAppUrl, spreadsheetId);
				foreach (var googleSheetInfo in googleSheetInfos)
				{
					var sheetName = googleSheetInfo.SheetName;
					var sheetId = googleSheetInfo.SheetId;

					var sheetInfo = spreadsheetInfo.FindSheetInfo(sheetId);

					if (sheetInfo is null)
						sheetInfo = spreadsheetInfo.CreateSheetInfo(sheetId);

					sheetInfo.SetName(spreadsheetName, sheetName);
				}

				var removeCount = spreadsheetInfo.SheetInfos.Count - googleSheetInfos.Length;
				if (removeCount > 0)
					spreadsheetInfo.RemoveSheetInfo(removeCount);

				spreadsheetInfo.OrderByIsUsing();
				Debug.Log($"[GoogleSpreadsheetLoader::UpdateSpreadsheetPreview] {spreadsheetName} update end.");
			}

			EditorUtility.SetDirty(this);
			Debug.Log($"[GoogleSpreadsheetLoader::UpdateSpreadsheetPreview] Spreadsheets update end. =====================================");
		}

		[ContextMenu("Update All Spreadsheet Contents")]
		public async Task UpdateAllUsedSheetContentInfos()
		{
			if (_isBusy)
				return;

			Debug.Log("[GoogleSpreadsheetLoader::UpdateAllUsedSheetContentInfos] SheetContents update start. ============================");

			_isBusy = true;

			CheckIsSheetContentRemoved();

			foreach (var spreadsheetInfo in _spreadsheetInfos)
			{
				var spreadsheetInfoId = spreadsheetInfo.GetId();
				var spreadsheetContentInfo = FindUsedSpreadSheetContentInfo(spreadsheetInfoId);

				if (spreadsheetContentInfo is null)
					spreadsheetContentInfo = CreateUsedSpreadSheetContentInfo(spreadsheetInfoId);

				var sheetContentPath = spreadsheetInfo.SheetContentPath;

				if (sheetContentPath != spreadsheetContentInfo.SheetContentPath)
					spreadsheetContentInfo.SetSheetContentPath(sheetContentPath);

				var spreadsheetId = spreadsheetInfo.SpreadsheetId;
				var spreadSheetName = spreadsheetInfo.SpreadsheetName;

				foreach (var sheetInfo in spreadsheetInfo.SheetInfos)
				{
					var sheetInfoId = sheetInfo.Id;
					var sheetId = sheetInfo.SheetId;

					var sheetContentInfo = spreadsheetContentInfo.FindSheetContentInfo(sheetInfoId);
					if (sheetInfo.IsUsing)
					{
						if (sheetContentInfo is null)
							sheetContentInfo = spreadsheetContentInfo.CreateSheetContentInfo(sheetInfoId, spreadsheetId, sheetId, spreadSheetName);

						await UpdateSheetContentInfo(sheetContentInfo);
						continue;
					}

					if (sheetContentInfo != null)
						sheetContentInfo.Remove();
				}
			}


			EditorUtility.SetDirty(this);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			_isBusy = false;
			Debug.Log("[GoogleSpreadsheetLoader::UpdateAllUsedSheetContentInfos] SheetContents update end. ==============================");
		}

		public void RemoveAllUsedSheetContentInfos()
		{
			var sheetContentInfos = _usedSpreadsheetContentInfos.ToArray();
			_usedSpreadsheetContentInfos.Clear();

			foreach (var sheetContentInfo in sheetContentInfos)
				sheetContentInfo.RemoveAll();
		}

		public void ResetBusy()
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