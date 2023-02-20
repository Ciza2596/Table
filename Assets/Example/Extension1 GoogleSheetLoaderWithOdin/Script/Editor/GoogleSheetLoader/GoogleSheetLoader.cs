using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleHelper;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoogleSheetLoader.Editor
{
    [CreateAssetMenu(fileName = "GoogleSheetLoader", menuName = "GoogleSheetLoader")]
    public class GoogleSheetLoader : ScriptableObject
    {
        //private variable
        [Title("Google服務設定", "執行Google apps script位置設定")]
        private string[] _webServices;

        [SerializeField] private int _currentWebServiceIndex;

        [Title("靜態表設定", "設定需要從雲端下載的表單")] [VerticalGroup("SpreadsheetsSetting")]
        private SheetInfo[] _sheetInfos;

        [TableList(IsReadOnly = true)] private  List<SheetContentInfo> _sheetContentInfos = new List<SheetContentInfo>();


        private GoogleHelper.GoogleHelper _googleHelper = new GoogleHelper.GoogleHelper();


        //public method
        public async Task UpdateSubSheetContent(SubSheetContentInfo subSheetContentInfo)
        {
            subSheetContentInfo.SetIsBusy(true);

            var webService = _webServices[_currentWebServiceIndex];
            var sheetId = subSheetContentInfo.SheetId;
            var subSheetId = subSheetContentInfo.SubSheetId;

            var csv = await GoogleGetCSV(webService, sheetId, subSheetId);
            subSheetContentInfo.Initialize(csv);

            subSheetContentInfo.SetIsBusy(false);
        }


        //private method

        /*/// <summary>
        /// 從google更新靜態表
        /// </summary>
        [VerticalGroup("SpreadsheetsSetting")]
        [Button("下載/更新所有表單")]
        private async void GetSpreadsheets()
        {
            if (SpreadsheetPageGids == null || SpreadsheetPageGids.Count <= 0) return;

            Debug.Log("[ContentManager:GetSpreadsheets] Start check Spreadsheets....");

            foreach (PageGID sheet in SpreadsheetPageGids)
            {
                string spreadsheetId = sheet.ParentSheet;

                if (string.IsNullOrEmpty(spreadsheetId)) continue;

                string result = await GoogleGetSpreadsheets(WebServices[CurrentWebServiceURL], spreadsheetId);
                List<object> jsonResult = Json.Deserialize(result) as List<object>;

                foreach (List<object> sheetInfo in jsonResult)
                {
                    string sheetName = sheetInfo[0].ToString();
                    string gid = sheetInfo[1].ToString();

                    InfoGID infoGID = sheet.Sheets.FirstOrDefault(item => item.Gid == gid);
                    if (infoGID == null)
                    {
                        sheet.Sheets.Add(new InfoGID() { Gid = gid, Description = sheetName });
                        Debug.Log($"[ContentManager:GetSpreadsheets] Add new sheet GID {gid} : {sheetName}.");
                    }
                    else
                    {
                        if (infoGID.Description != sheetName)
                        {
                            infoGID.Description = sheetName;
                            Debug.Log($"[ContentManager:GetSpreadsheets] Sheet name change {gid} : {sheetName}.");
                        }
                    }
                }

                sheet.Sheets = sheet.Sheets.OrderByDescending(sheetInfo => sheetInfo.IsUsing).ToList();
            }

            Debug.Log("[ContentManager:GetSpreadsheets] Spreadsheets is update.");
        }

        /// <summary>
        /// 更新Scriptable Content
        /// 讀取SpreadsheetPageGids中 is using 為true的物件
        /// </summary>
        [Button("更新Scriptable Content")]
        [ButtonGroup("ContentList")]
        [GUIColor(0, 1, 0)]
        [DisableIf("IsBusy")]
        private async void UpdateUsedContentList()
        {
            if (IsBusy) return;

            IsBusy = true;

            List<Task> updateTasks = new List<Task>();

            foreach (PageGID page in SpreadsheetPageGids)
            {
                foreach (InfoGID sheet in page.Sheets)
                {
                    if (sheet.IsUsing)
                    {
                        SpreadsheetContentInfo contentInfo =
                            UsedContent.FirstOrDefault(item => item.Content.PageGid == sheet.Gid);
                        if (contentInfo == null)
                        {
                            SpreadsheetContent content =
                                ScriptableObjectUtility.CreateScriptableObject<SpreadsheetContent>(
                                    ScriptableContentPath, sheet.Description, true);
                            content.WebService = WebServices[CurrentWebServiceURL];
                            content.PageGid = sheet.Gid;
                            content.SpreadsheetID = page.ParentSheet;
                            contentInfo = new SpreadsheetContentInfo() { Content = content, _contentManager = this };
                            UsedContent.Add(contentInfo);

                            EditorUtility.SetDirty(this);

                            Debug.Log(
                                $"[ContentManager:UpdateUsedContentList] Create new content {sheet.Description}.");
                        }

                        updateTasks.Add(UpdateContentSheet(contentInfo));
                    }
                }
            }

            await Task.WhenAll(updateTasks);

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            IsBusy = false;
        }*/

        [Button("清除所有Scriptable Content")]
        [ButtonGroup("ContentList")]
        [GUIColor(1, 0, 0)]
        [DisableIf("IsBusy")]
        private void RemoveAllScriptableContent()
        {
            var sheetContentInfos = _sheetContentInfos.ToArray();
            _sheetContentInfos.Clear();


            foreach (var sheetContentInfo in sheetContentInfos)
                sheetContentInfo.RemoveAll();
            
            Debug.Log("[ContentManager:RemoveAllScriptableContent] All content is clear.");
        }


        [GUIColor(0, 1, 1)]
        [Button("ResetBusy")]
        private void ResetBusy()
        {
            var sheetContentInfos = _sheetContentInfos.ToArray();
            foreach (var sheetContentInfo in sheetContentInfos)
            foreach (var subSheetContentInfo in sheetContentInfo.SubSheetContentInfos)
                subSheetContentInfo.SetIsBusy(false);
        }


        #region Google Helper

        /// <summary>
        /// 取的指定表單全部分頁資料
        /// </summary>
        /// <param name="service"></param>
        /// <param name="spreadsheet"></param>
        /// <returns>Json結構GID:SheetName</returns>
        public async Task<string> GoogleGetSpreadsheets(string service, string spreadsheet)
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
        public async Task<string> GoogleGetCSV(string service, string sheetId, string subSheetId)
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
        public async Task<string> GoogleGetPageName(string service, string sheetId, string subSheetId)
        {
            var action = "GetSheetName";
            var parameters = new Dictionary<string, string>
            {
                { "key", sheetId },
                { "gid", subSheetId },
            };

            var requestURL = new RequestURL(service, action, parameters);
            return await _googleHelper.StartDownload(requestURL);
        }

        /// <summary>
        /// 打開指定的表單頁面
        /// </summary>
        public void GoogleOpenURL(string service, string sheetId, string subSheetId, int row = 0, int column = 0)
        {
            var request = $"{service}?key={sheetId}&gid={subSheetId}&action=GetRawCSV";

            if (row > 0 && column > 0)
                request += string.Format("&row={0}&column={1}", row, column);

            Application.OpenURL(request);
        }

        // /// <summary>
        // /// 取得目前服務清單，編輯器使用
        // /// </summary>
        // /// <returns></returns>
        // private ValueDropdownList<int> GetServiceList()
        // {
        //     var valueList = new ValueDropdownList<int>();
        //
        //     if (WebServices == null)
        //         return null;
        //
        //     for (int i = 0; i < WebServices.Count; i++)
        //     {
        //         valueList.Add(WebServices[i].Replace("/", @"\"), i);
        //     }
        //
        //     return valueList;
        // }

        #endregion
    }
}