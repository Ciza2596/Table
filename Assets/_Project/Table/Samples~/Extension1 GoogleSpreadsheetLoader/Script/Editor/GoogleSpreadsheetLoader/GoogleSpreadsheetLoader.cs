using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
	[CreateAssetMenu(fileName = "GoogleSpreadsheetLoader", menuName = "Ciza/Table/GoogleSpreadsheetLoader")]
	public class GoogleSpreadsheetLoader : ScriptableObject
	{
		//private variable
		[VerticalGroup("Service")]
		[SerializeField]
		private string _webAppUrl;

		[Title("Spreadsheet preview")]
		[VerticalGroup("SpreadsheetPreview")]
		[SerializeField]
		private List<SpreadsheetInfo> _spreadsheetInfos;

		[Title("Used Sheet Content")]
		[SerializeField]
		private List<SpreadsheetContentInfo> _usedSpreadsheetContentInfos = new List<SpreadsheetContentInfo>();

		private bool                        _isBusy;
		private GoogleSpreadsheetGasHandler _googleSpreadsheetGasHandler = new GoogleSpreadsheetGasHandler();

		//public method
		public async Task UpdateSheetContentInfo(SheetContentInfo sheetContentInfo)
		{
			sheetContentInfo.SetIsBusy(true);

			var spreadSheetId = sheetContentInfo.SpreadSheetId;
			var sheetId       = sheetContentInfo.SheetId;

			var spreadsheetName = await _googleSpreadsheetGasHandler.GetSpreadsheetName(_webAppUrl, spreadSheetId);
			var sheetName       = await _googleSpreadsheetGasHandler.GetSheetName(_webAppUrl, spreadSheetId, sheetId);

			var spreadsheetInfo =
				_spreadsheetInfos.Find(spreadsheetInfo => spreadsheetInfo.SpreadsheetId == spreadSheetId);

			var sheetContentPath = spreadsheetInfo.SheetContentPath;
			var folderPath       = PathHelper.GetFolderPath(sheetContentPath, spreadsheetName);

			var csv = await _googleSpreadsheetGasHandler.GetGoogleSheetCsv(_webAppUrl, spreadSheetId, sheetId);

			sheetContentInfo.Update(sheetName, folderPath, csv);

			sheetContentInfo.SetIsBusy(false);
		}

		//private method
		[Title("Google Web Service")]
		[PropertyOrder(-100)]
		[Button("Open Google App Script Web.")]
		[VerticalGroup("Service")]
		private void OpenGoogleScriptPage() =>
			Application.OpenURL("https://www.google.com/script/start/");

		[VerticalGroup("SpreadsheetPreview")]
		[Button("Update Spreadsheet Preview")]
		private async void UpdateSpreadsheets()
		{
			if (_spreadsheetInfos is null || _spreadsheetInfos.Count <= 0) return;

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

				spreadsheetInfo.OrderByIsUsing();
			}

			Debug.Log("[GoogleSpreadsheetLoader::UpdateSpreadsheets] Spreadsheets is updated.");
		}

		[Button("Update All Used Sheet Contents")]
		[ButtonGroup("ContentList")]
		[GUIColor(0, 1, 0)]
		[DisableIf("_isBusy")]
		private async void UpdateAllUsedSheetContentInfos()
		{
			if (_isBusy) return;

			Debug.Log("[GoogleSpreadsheetLoader::UpdateAllUsedSheetContentInfos] Start update all used sheet contents....");

			_isBusy = true;

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
				var spreadSheetName = spreadsheetInfo.SpreadSheetName;

				foreach (var sheetInfo in spreadsheetInfo.SheetInfos)
				{
					var sheetInfoId = sheetInfo.Id;
					var sheetId     = sheetInfo.SheetId;

					var sheetContentInfo = spreadsheetContentInfo.FindSheetContentInfo(sheetInfoId);
					if (sheetInfo.IsUsing)
					{
						if (sheetContentInfo is null)
							sheetContentInfo =
								spreadsheetContentInfo.CreateSheetContentInfo(sheetInfoId, spreadsheetId, sheetId,
								                                              spreadSheetName,
								                                              this);

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

		[Button("Remove All Used Sheet Content")]
		[ButtonGroup("ContentList")]
		[GUIColor(1, 0, 0)]
		[DisableIf("_isBusy")]
		private void RemoveAllUsedSheetContentInfos()
		{
			var sheetContentInfos = _usedSpreadsheetContentInfos.ToArray();
			_usedSpreadsheetContentInfos.Clear();


			foreach (var sheetContentInfo in sheetContentInfos)
				sheetContentInfo.RemoveAll();
		}

		[GUIColor(0, 1, 1)]
		[Button("ResetBusy")]
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
			_usedSpreadsheetContentInfos.Find(spreadsheetContentInfo =>
				                                  spreadsheetContentInfo.SpreadsheetInfoId == spreadsheetInfoId);

		/// <summary>
		/// 打開指定的表單頁面
		/// </summary>
		private void GoogleOpenUrl(string service, string spreadsheetId, string sheetId, int row = 0, int column = 0)
		{
			var request = $"{service}?key={spreadsheetId}&gid={sheetId}&action=GetRawCSV";

			if (row > 0 && column > 0)
				request += string.Format("&row={0}&column={1}", row, column);

			Application.OpenURL(request);
		}
	}
}
