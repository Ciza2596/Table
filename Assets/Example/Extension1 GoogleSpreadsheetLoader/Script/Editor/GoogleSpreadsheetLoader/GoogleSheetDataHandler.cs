using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleHelper;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class GoogleSheetDataHandler
    {
        //private variable
        [SerializeField] public List<GoogleSheetCsv> _googleSheetCsvs = new List<GoogleSheetCsv>();

        private GoogleHelper.GoogleHelper _googleHelper = new GoogleHelper.GoogleHelper();


        //public method
        public async Task<GoogleSheetInfo[]> GetGoogleSheetInfos(string service, string[] spreadsheetIds)
        {
            var googleSheetDatas = new List<GoogleSheetInfo>();

            foreach (var spreadsheetId in spreadsheetIds)
                await GoogleGetSheets(service, spreadsheetId, googleSheetDatas);

            return googleSheetDatas.ToArray();
        }


        public Task UpdateAllGoogleSheetCsvs(string service, string[] spreadsheetIds)
        {
            return Task.CompletedTask;
        }


        //private variable
        private async Task GoogleGetSheets(string service, string spreadsheetId, List<GoogleSheetInfo> googleSheetDatas)
        {
            var action = "GetSpreadsheets";
            var parameters = new Dictionary<string, string>
            {
                { "key", spreadsheetId }
            };

            var requestURL = new RequestURL(service, action, parameters);
            var result = await _googleHelper.StartDownload(requestURL);
            var deserializeResults= MiniJson.Deserialize(result) as List<object>;

            foreach (var deserializeResult in deserializeResults)
            {
                var sheetData = deserializeResult as List<Object>;

                var spreadSheetName = sheetData[0].ToString();
                var sheetName = sheetData[1].ToString();
                var sheetId = sheetData[2].ToString();

                var googleSheetInfo = new GoogleSheetInfo(spreadSheetName, sheetName, sheetId);
                googleSheetDatas.Add(googleSheetInfo);
            }
        }
    }


    [Serializable]
    public class GoogleSheetInfo
    {
        [SerializeField] private string _spreadSheetName;
        [SerializeField] private string _sheetName;
        [SerializeField] private string _sheetId;


        public GoogleSheetInfo(string spreadSheetName, string sheetName, string sheetId)
        {
            _spreadSheetName = spreadSheetName;
            _sheetName = sheetName;
            _sheetId = sheetId;
        }

        public string SpreadSheetName => _spreadSheetName;
        public string SheetName => _sheetName;
        public string SheetId => _sheetId;
    }

    [Serializable]
    public class GoogleSheetCsv
    {
        [SerializeField] private string _spreadsheetId;
        [SerializeField] private string _sheetId;
        [SerializeField] private string _csv;


        public GoogleSheetCsv(string spreadsheetId, string sheetId, string csv)
        {
            _spreadsheetId = spreadsheetId;
            _sheetId = sheetId;
            _csv = csv;
        }

        public string SpreadsheetId => _spreadsheetId;
        public string SheetId => _sheetId;
        public string Csv => _csv;
    }
}