using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSheetLoader.Editor
{
    [Serializable]
    public class SheetInfo
    {
        //private variable
        [LabelText("Sheet Content存放路徑(建立物件時使用)")]
        public string _sheetContentPath = "Assets/_Project/AAS/ScriptableObjects/Tables/";

        [Space] [LabelText("表單位址")] private string _sheetId;

        [TableList(HideToolbar = true, AlwaysExpanded = true)]
        private SubSheetInfo[] _subSheetInfos;

        //constructor
        public SheetInfo(string sheetId, SubSheetInfo[] subSheetInfos)
        {
            _sheetId = sheetId;
            _subSheetInfos = subSheetInfos;
        }

        //public variable
        public string SheetContentPath => _sheetContentPath;
        public string SheetId => _sheetId;
        public SubSheetInfo[] SubSheetInfos => _subSheetInfos;
    }
}