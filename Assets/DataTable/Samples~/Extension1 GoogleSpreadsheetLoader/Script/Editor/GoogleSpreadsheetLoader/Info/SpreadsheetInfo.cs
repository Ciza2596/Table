using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class SpreadsheetInfo
    {
        //private variable

        [VerticalGroup("Web")] [PropertyOrder(0)] [SerializeField]
        private string _spreadsheetId;

        [VerticalGroup("Web")] [PropertyOrder(10)] [SerializeField]
        private string _sheetContentPath = "Assets/Table";

        [Space] [ReadOnly] [SerializeField] private string _spreadSheetName;

        [Space] [TableList(HideToolbar = true, AlwaysExpanded = true)] [SerializeField]
        private List<SheetInfo> _sheetInfos;

        [HideInInspector] [SerializeField] private string _id;

        //public variable

        public string SpreadSheetName => _spreadSheetName;

        public IReadOnlyList<SheetInfo> SheetInfos => _sheetInfos;

        public string SheetContentPath => _sheetContentPath;
        public string SpreadsheetId => _spreadsheetId;


        //public method
        public string GetId()
        {
            if (string.IsNullOrWhiteSpace(_id))
                _id = Guid.NewGuid().ToString();

            return _id;
        }

        public SheetInfo FindSheetInfo(string sheetId)
        {
            var sheetInfo = _sheetInfos.Find(sheetInfo => sheetInfo.SheetId == sheetId);
            return sheetInfo;
        }

        public SheetInfo CreateSheetInfo(string sheetId)
        {
            var sheetInfo = new SheetInfo(sheetId);
            _sheetInfos.Add(sheetInfo);

            return sheetInfo;
        }

        public void OrderByIsUsing() =>
            _sheetInfos = _sheetInfos.OrderByDescending(sheetInfo => sheetInfo.IsUsing).ToList();

        public void SetSpreadSheetName(string spreadSheetName) =>
            _spreadSheetName = spreadSheetName;

        //private method
        [PropertyOrder(20)]
        [VerticalGroup("Web")]
        [Button("Open Spreadsheet Web.")]
        private void OpenSpreadsheetWeb() =>
            Application.OpenURL($"https://docs.google.com/spreadsheets/d/{_spreadsheetId}/edit#gid=0");
        
    }
}