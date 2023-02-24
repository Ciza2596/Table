using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleHelper;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [CreateAssetMenu(fileName = "GoogleSpreadsheetLoader", menuName = "DataTable/GoogleSpreadsheetLoader")]
    public class GoogleSpreadsheetLoader : ScriptableObject
    {
        //private variable
        [VerticalGroup("Service")] [SerializeField]
        private string _webAppUrl;

        [Title("Spreadsheet preview")] [VerticalGroup("SpreadsheetPreview")] [SerializeField]
        private List<SpreadsheetInfo> _spreadsheetInfos;

        [Title("Used Sheet Content")] [ReadOnly] [SerializeField]
        private List<SpreadsheetContentInfo> _usedSpreadsheetContentInfos = new List<SpreadsheetContentInfo>();


        private bool _isBusy;
        private GoogleHelper.GoogleHelper _googleHelper = new GoogleHelper.GoogleHelper();


        //public method
        public async Task UpdateSheetContentInfo(SheetContentInfo sheetContentInfo)
        {
            sheetContentInfo.SetIsBusy(true);

            var spreadSheetId = sheetContentInfo.SpreadSheetId;
            var sheetId = sheetContentInfo.SheetId;

            var sheetName = await GoogleGetSheetName(_webAppUrl, spreadSheetId, sheetId);
            var csv = await GoogleGetCsv(_webAppUrl, spreadSheetId, sheetId);
            sheetContentInfo.Update(sheetName, csv);

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

            Debug.Log("[ContentManager:GetSpreadsheets] Start check Spreadsheets....");

            foreach (var spreadsheetInfo in _spreadsheetInfos)
            {
                var spreadsheetId = spreadsheetInfo.SpreadsheetId;

                if (string.IsNullOrEmpty(spreadsheetId))
                    continue;

                var sheets = await GoogleGetSheets(_webAppUrl, spreadsheetId);

                foreach (List<object> sheet in sheets)
                {
                    var sheetName = sheet[0].ToString();
                    var sheetId = sheet[1].ToString();

                    var sheetInfo = spreadsheetInfo.FindSheetInfo(sheetId);

                    if (sheetInfo is null)
                        sheetInfo = spreadsheetInfo.CreateSheetInfo(sheetId);

                    if (sheetInfo.Name != sheetName)
                        sheetInfo.SetName(sheetName);
                }

                spreadsheetInfo.OrderByIsUsing();
            }

            Debug.Log("[ContentManager:GetSpreadsheets] Spreadsheets is update.");
        }


        [Button("Update All Used Sheet Content")]
        [ButtonGroup("ContentList")]
        [GUIColor(0, 1, 0)]
        [DisableIf("_isBusy")]
        private async void UpdateAllUsedSheetContentInfos()
        {
            if (_isBusy) return;

            _isBusy = true;

            var sheetContentInfoUpdates = new List<Task>();

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

                foreach (var sheetInfo in spreadsheetInfo.SheetInfos)
                {
                    var sheetInfoId = sheetInfo.Id;
                    var sheetId = sheetInfo.SheetId;

                    var sheetContentInfo = spreadsheetContentInfo.FindSheetContentInfo(sheetInfoId);
                    if (sheetInfo.IsUsing)
                    {
                        if (sheetContentInfo is null)
                            sheetContentInfo =
                                spreadsheetContentInfo.CreateSheetContentInfo(sheetInfoId, spreadsheetId, sheetId, "",
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

            _isBusy = false;
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

            Debug.Log("[ContentManager:RemoveAllScriptableContent] All content is clear.");
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


        #region Google Helper

        /// <summary>
        /// 取的指定表單全部分頁資料
        /// </summary>
        /// <param name="service"></param>
        /// <param name="spreadsheetId"></param>
        /// <returns>Json結構GID:SheetName</returns>
        private async Task<List<object>> GoogleGetSheets(string service, string spreadsheetId)
        {
            var action = "GetSpreadsheets";
            var parameters = new Dictionary<string, string>
            {
                { "key", spreadsheetId }
            };

            var requestURL = new RequestURL(service, action, parameters);
            var result = await _googleHelper.StartDownload(requestURL);
            var deserialize = MiniJson.Deserialize(result) as List<object>;
            return deserialize;
        }

        /// <summary>
        /// 取得CSV
        /// </summary>
        /// <param name="service">服務位址</param>
        /// <param name="sheetId">表單ID</param>
        /// <param name="spreadSheetId">分頁ID</param>
        /// <returns>pageCSV</returns>
        private async Task<string> GoogleGetCsv(string service, string spreadSheetId, string sheetId)
        {
            var action = "GetRawCsv";
            var parameters = new Dictionary<string, string>
            {
                { "key", spreadSheetId },
                { "gid", sheetId },
            };

            var requestURL = new RequestURL(service, action, parameters);
            return await _googleHelper.StartDownload(requestURL);
        }

        // /// <summary>
        // /// 取得表單名稱
        // /// </summary>
        // private async Task<string> GoogleGetSpreadSheetName(string service, string spreadsheetId)
        // {
        //     var action = "GetSpreadSheetName";
        //     var parameters = new Dictionary<string, string>
        //     {
        //         { "key", spreadsheetId },
        //         { "gid", sheetId },
        //     };
        //
        //     var requestURL = new RequestURL(service, action, parameters);
        //     return await _googleHelper.StartDownload(requestURL);
        // }


        /// <summary>
        /// 取得分頁名稱
        /// </summary>
        private async Task<string> GoogleGetSheetName(string service, string spreadsheetId, string sheetId)
        {
            var sheets = await GoogleGetSheets(service, spreadsheetId);

            var sheet = sheets.Find(sheet =>
            {
                var sheetData = sheet as List<object>;
                return sheetData[1].ToString() == sheetId;
            });
            
            
            var sheetData = sheet as List<object>;
            return sheetData[0].ToString();
            // var action = "GetSheetName";
            // var parameters = new Dictionary<string, string>
            // {
            //     { "key", spreadsheetId },
            //     { "gid", sheetId },
            // };
            //
            // var requestURL = new RequestURL(service, action, parameters);
            // return await _googleHelper.StartDownload(requestURL);
        }

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

        #endregion
    }
}