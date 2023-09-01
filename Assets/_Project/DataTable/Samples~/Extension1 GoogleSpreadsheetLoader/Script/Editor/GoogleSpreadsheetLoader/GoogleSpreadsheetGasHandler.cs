using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleHelper;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class GoogleSpreadsheetGasHandler
    {
        //private variable
        [SerializeField] public List<GoogleSheetCsv> _googleSheetCsvs = new List<GoogleSheetCsv>();

        private GoogleHelper.GoogleHelper _googleHelper = new GoogleHelper.GoogleHelper();


        //public method

        public async Task<string> GetSpreadsheetName(string service, string spreadsheetId)
        {
            var action = "GetSpreadsheetName";
            var parameters = new Dictionary<string, string>
            {
                { "key", spreadsheetId },
            };

            var requestURL = new RequestURL(service, action, parameters);
            return await _googleHelper.StartDownload(requestURL);
        }
        
        public async Task<string> GetSheetName(string service, string spreadsheetId, string sheetId)
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

        public async Task<GoogleSheetInfo[]> GetGoogleSheetInfos(string service, string spreadsheetId)
        {
            var googleSheetDatas = new List<GoogleSheetInfo>();

            await GoogleGetSheets(service, spreadsheetId, googleSheetDatas);

            return googleSheetDatas.ToArray();
        }

        public async Task<string> GetGoogleSheetCsv(string service, string spreadSheetId, string sheetId)
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
            var deserializeResults = MiniJson.Deserialize(result) as List<object>;

            foreach (var deserializeResult in deserializeResults)
            {
                var sheetData = deserializeResult as List<object>;

                var sheetId = sheetData[0].ToString();
                var sheetName = sheetData[1].ToString();

                var googleSheetInfo = new GoogleSheetInfo(sheetId, sheetName);
                googleSheetDatas.Add(googleSheetInfo);
            }
        }
    }


    [Serializable]
    public class GoogleSheetInfo
    {
        [SerializeField] private string _sheetId;
        [SerializeField] private string _sheetName;

        public GoogleSheetInfo(string sheetId, string sheetName)
        {
            _sheetId = sheetId;
            _sheetName = sheetName;
        }

        public string SheetId => _sheetId;
        public string SheetName => _sheetName;
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