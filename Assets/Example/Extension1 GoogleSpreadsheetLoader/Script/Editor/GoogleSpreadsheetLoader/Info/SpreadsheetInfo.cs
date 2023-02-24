using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class SpreadsheetInfo
    {
        //private variable
        [Space] [LabelText("表單位址")] [SerializeField]
        private string _spreadsheetId;
        
        [LabelText("Sheet Content存放路徑(建立物件時使用)")] [SerializeField]
        private string _sheetContentPath = "Assets/_Project/AAS/ScriptableObjects/Tables/";

        [TableList(HideToolbar = true, AlwaysExpanded = true)] [SerializeField]
        private SheetInfo[] _sheetInfos;
        
        private readonly string _id;

        //constructor
        public SpreadsheetInfo(string spreadsheetId, SheetInfo[] sheetInfos)
        {
            _spreadsheetId = spreadsheetId;
            _sheetInfos = sheetInfos;
            _id = Guid.NewGuid().ToString();
        }

        //public variable
        public string SheetContentPath => _sheetContentPath;
        public string SpreadsheetId => _spreadsheetId;
        public SheetInfo[] SheetInfos => _sheetInfos;

        public string Id => _id;
    }
}