using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class SpreadsheetContentInfo
    {
        //private variable
        [LabelText("Sheet Content存放路徑(建立物件時使用)")] [ReadOnly] [SerializeField]
        private string _sheetContentPath;

        [ReadOnly] [SerializeField]
        private List<SheetContentInfo> _sheetContentInfos = new List<SheetContentInfo>();


        //public variable
        public string SheetContentPath => _sheetContentPath;

        public SheetContentInfo[] SheetContentInfos => _sheetContentInfos.ToArray();


        //public method
        public void UpdateAll()
        {
            var subSheetContentInfos = SheetContentInfos;

            foreach (var subSheetContentInfo in subSheetContentInfos)
                subSheetContentInfo.Update();
        }

        public void RemoveAll()
        {
            var subSheetContentInfos = SheetContentInfos;

            foreach (var subSheetContentInfo in subSheetContentInfos)
            {
                _sheetContentInfos.Remove(subSheetContentInfo);
                subSheetContentInfo.Remove();
            }
        }


        public void SetSheetContentPath(string sheetContentPath) => _sheetContentPath = sheetContentPath;

        public void AddSubSheetContentInfo(SheetContentInfo sheetContentInfo) =>
            _sheetContentInfos.Add(sheetContentInfo);
    }
}