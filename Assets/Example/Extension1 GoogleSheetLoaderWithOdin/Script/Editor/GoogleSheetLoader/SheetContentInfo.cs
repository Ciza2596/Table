using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSheetLoader.Editor
{
    [Serializable]
    public class SheetContentInfo
    {
        //private variable
        [LabelText("Sheet Content存放路徑(建立物件時使用)")] [ReadOnly] [SerializeField]
        private string _sheetContentPath;

        [ReadOnly] [SerializeField]
        private List<SubSheetContentInfo> _subSheetContentInfos = new List<SubSheetContentInfo>();


        //public variable
        public string SheetContentPath => _sheetContentPath;

        public SubSheetContentInfo[] SubSheetContentInfos => _subSheetContentInfos.ToArray();


        //public method
        public void SetSheetContentPath(string sheetContentPath) => _sheetContentPath = sheetContentPath;

        public void AddSubSheetContentInfo(SubSheetContentInfo subSheetContentInfo) =>
            _subSheetContentInfos.Add(subSheetContentInfo);

        public void RemoveSubSheetContentInfo(SubSheetContentInfo subSheetContentInfo) =>
            _subSheetContentInfos.Remove(subSheetContentInfo);
    }
}