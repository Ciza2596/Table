using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSheetLoader.Editor
{
    [CreateAssetMenu(fileName = "GoogleSheetLoader", menuName = "GoogleSheetLoader")]
    public class GoogleSheetLoader : ScriptableObject
    {
        [Title("Google服務設定", "執行Google apps script位置設定")]
        private string[] _webServices;

        [SerializeField] private int _currentWebServiceIndex;
        
        [Title("靜態表設定", "設定需要從雲端下載的表單")] [VerticalGroup("SpreadsheetsSetting")]
        private SheetInfo[] _sheetInfos;


        [TableList(IsReadOnly = true)]
        private List<SheetContentInfo> _sheetContentInfos;

    }

    [Serializable]
    public class SheetInfo
    {
        //private variable
        [LabelText("Sheet Content存放路徑(建立物件時使用)")]
        public string _sheetContentPath = "Assets/_Project/AAS/ScriptableObjects/ContentSheet/";
        
        [LabelText("表單位址")] private string _sheetId;

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

    [Serializable]
    public class SubSheetInfo
    {
        //private variable
        [TableColumnWidth(100)] [ReadOnly] [SerializeField]
        private string _id;

        [ReadOnly] [SerializeField] private string _description;

        [SerializeField] private bool _isUsing;

        //constructor
        public SubSheetInfo(string id, string description)
        {
            _id = id;
            _description = description;
        }


        //public variable
        public string Id => _id;
        public string Description => _description;
        public bool IsUsing => _isUsing;
    }

    [Serializable]
    public class SheetContentInfo
    {
        private List<SubSheetContentInfo> _subSheetContentInfos;
    }
    
    
    
    [Serializable]
    public class SubSheetContentInfo
    {
        
    }
}