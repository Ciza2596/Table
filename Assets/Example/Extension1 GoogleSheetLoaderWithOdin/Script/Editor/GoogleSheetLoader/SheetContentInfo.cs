using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
        public void UpdateAll()
        {
            var subSheetContentInfos = SubSheetContentInfos;

            foreach (var subSheetContentInfo in subSheetContentInfos)
                subSheetContentInfo.Update();
        }

        public void RemoveAll()
        {
            var subSheetContentInfos = SubSheetContentInfos;

            foreach (var subSheetContentInfo in subSheetContentInfos)
            {
                _subSheetContentInfos.Remove(subSheetContentInfo);
                subSheetContentInfo.Remove();
            }
        }


        public void SetSheetContentPath(string sheetContentPath) => _sheetContentPath = sheetContentPath;

        public void AddSubSheetContentInfo(SubSheetContentInfo subSheetContentInfo) =>
            _subSheetContentInfos.Add(subSheetContentInfo);
    }
}