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
        [Space] [LabelText("表單位址")] [SerializeField]
        private string _spreadsheetId;
        
        [LabelText("Sheet Content存放路徑(建立物件時使用)")] [SerializeField]
        private string _sheetContentPath = "Assets/_Project/AAS/ScriptableObjects/Tables/";

        [TableList(HideToolbar = true, AlwaysExpanded = true)] [SerializeField]
        private List<SheetInfo> _sheetInfos;
        
        private readonly string _id;

        //constructor
        public SpreadsheetInfo(string spreadsheetId, List<SheetInfo> sheetInfos)
        {
            _id = Guid.NewGuid().ToString();
            
            _spreadsheetId = spreadsheetId;
            _sheetInfos = sheetInfos;
        }

        //public variable
        public string Id => _id;
        
        public IReadOnlyList<SheetInfo> SheetInfos => _sheetInfos;

        public string SheetContentPath => _sheetContentPath;
        public string SpreadsheetId => _spreadsheetId;
        
        //public method
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
        
    }
}