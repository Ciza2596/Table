using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class SpreadsheetInfo
    {
        //private variable
        [LabelText("Sheet Content存放路徑(建立物件時使用)")]
        public string _sheetContentPath = "Assets/_Project/AAS/ScriptableObjects/Tables/";

        [Space] [LabelText("表單位址")] private string _spreadsheetId;

        [TableList(HideToolbar = true, AlwaysExpanded = true)]
        private SheetInfo[] _sheetInfos;

        //constructor
        public SpreadsheetInfo(string spreadsheetId, SheetInfo[] sheetInfos)
        {
            _spreadsheetId = spreadsheetId;
            _sheetInfos = sheetInfos;
        }

        //public variable
        public string SheetContentPath => _sheetContentPath;
        public string SpreadsheetId => _spreadsheetId;
        public SheetInfo[] SheetInfos => _sheetInfos;
    }
}