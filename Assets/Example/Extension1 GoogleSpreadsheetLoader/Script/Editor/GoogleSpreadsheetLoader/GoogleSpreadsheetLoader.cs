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
        [Title("Google服務設定", "執行Google apps script位置設定")] [SerializeField]
        private string[] _webServices;

        [SerializeField] private int _currentWebServiceIndex;

        [Title("靜態表設定", "設定需要從雲端下載的表單")] [VerticalGroup("SpreadsheetsSetting")] [SerializeField]
        private List<SpreadsheetInfo> _spreadsheetInfos;

        [TableList(IsReadOnly = true)] [SerializeField]
        private List<SpreadsheetContentInfo> _usedSpreadsheetContentInfos = new List<SpreadsheetContentInfo>();


        private bool _isBusy;
        private GoogleHelper.GoogleHelper _googleHelper = new GoogleHelper.GoogleHelper();


        //public method
        public async Task UpdateSheetContentInfo(SheetContentInfo sheetContentInfo)
        {
            sheetContentInfo.SetIsBusy(true);

            var webService = _webServices[_currentWebServiceIndex];
            var spreadSheetId = sheetContentInfo.SpreadSheetId;
            var sheetId = sheetContentInfo.SheetId;

            var sheetName =  await GoogleGetSheetName(webService, spreadSheetId, sheetId);
            var csv = await GoogleGetCsv(webService, spreadSheetId, sheetId);
            sheetContentInfo.Update(sheetName, csv);

            sheetContentInfo.SetIsBusy(false);
        }


        //private method
        [VerticalGroup("SpreadsheetsSetting")]
        [Button("下載/更新所有表單GID")]
        private async void UpdateSpreadsheets()
        {
            if (_spreadsheetInfos is null || _spreadsheetInfos.Count <= 0) return;

            Debug.Log("[ContentManager:GetSpreadsheets] Start check Spreadsheets....");

            foreach (var spreadsheetInfo in _spreadsheetInfos)
            {
                var spreadsheetId = spreadsheetInfo.SpreadsheetId;

                if (string.IsNullOrEmpty(spreadsheetId)) 
                    continue;

                var result = await GoogleGetSheets(_webServices[_currentWebServiceIndex], spreadsheetId);
                var sheets = MiniJson.Deserialize(result) as List<object>;

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
        
        
        
        [Button("更新Scriptable Content")]
        [ButtonGroup("ContentList")]
        [GUIColor(0, 1, 0)]
        [DisableIf("_isBusy")]
        private async void UpdateAllUsedContentInfos()
        {
            if (_isBusy) return;

            _isBusy = true;

            var sheetContentInfoUpdates = new List<Task>();

            foreach (var spreadsheetInfo in _spreadsheetInfos)
            {
                var spreadsheetInfoId = spreadsheetInfo.Id;
                var spreadsheetContentInfo = FindUsedSpreadSheetContentInfo(spreadsheetInfoId);

                if (spreadsheetContentInfo is null)
                    spreadsheetContentInfo = CreateUsedSpreadSheetContentInfo(spreadsheetInfoId);

                foreach (var sheetInfo in spreadsheetInfo.SheetInfos)
                {
                    var sheetInfoId = sheetInfo.Id;

                    var sheetContentInfo = spreadsheetContentInfo.FindSheetContentInfo(sheetInfoId);
                    if (sheetInfo.IsUsing)
                    {
                        if (sheetContentInfo is null)
                            sheetContentInfo = spreadsheetContentInfo.CreateSheetContentInfo(sheetInfoId,this);

                        sheetContentInfoUpdates.Add(sheetContentInfo.Update());
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


        [Button("清除所有Scriptable Content")]
        [ButtonGroup("ContentList")]
        [GUIColor(1, 0, 0)]
        [DisableIf("_isBusy")]
        private void RemoveAllUsedContentInfos()
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
        /// <param name="spreadsheet"></param>
        /// <returns>Json結構GID:SheetName</returns>
        public async Task<string> GoogleGetSheets(string service, string spreadsheet)
        {
            var action = "GetSpreadsheets";
            var parameters = new Dictionary<string, string>
            {
                { "key", spreadsheet }
            };

            var requestURL = new RequestURL(service, action, parameters);
            return await _googleHelper.StartDownload(requestURL);
        }

        /// <summary>
        /// 取得CSV
        /// </summary>
        /// <param name="service">服務位址</param>
        /// <param name="subSheetId">表單ID</param>
        /// <param name="sheetId">分頁ID</param>
        /// <returns>pageCSV</returns>
        public async Task<string> GoogleGetCsv(string service, string sheetId, string subSheetId)
        {
            var action = "GetRawCSV";
            var parameters = new Dictionary<string, string>
            {
                { "key", sheetId },
                { "gid", subSheetId },
            };

            var requestURL = new RequestURL(service, action, parameters);
            return await _googleHelper.StartDownload(requestURL);
        }

        /// <summary>
        /// 取得分頁名稱
        /// </summary>
        public async Task<string> GoogleGetSheetName(string service, string spreadsheetId, string sheetId)
        {
            var action = "GetSheetName";
            var parameters = new Dictionary<string, string>
            {
                { "key", spreadsheetId },
                { "gid", sheetId },
            };

            var requestURL = new RequestURL(service, action, parameters);
            return await _googleHelper.StartDownload(requestURL);
        }

        /// <summary>
        /// 打開指定的表單頁面
        /// </summary>
        public void GoogleOpenUrl(string service, string spreadsheetId, string sheetId, int row = 0, int column = 0)
        {
            var request = $"{service}?key={spreadsheetId}&gid={sheetId}&action=GetRawCSV";

            if (row > 0 && column > 0)
                request += string.Format("&row={0}&column={1}", row, column);

            Application.OpenURL(request);
        }

        #endregion
    }
}