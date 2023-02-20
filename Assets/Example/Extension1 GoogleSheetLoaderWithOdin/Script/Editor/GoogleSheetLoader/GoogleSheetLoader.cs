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

        [TableList(IsReadOnly = true)] private List<SheetContentInfo> _sheetContentInfos;


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