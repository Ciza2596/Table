using System;
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
    }

    [Serializable]
    public class PageId
    {
        //private variable
        [LabelText("表單位址")] private string _parentSheet;

        [TableList(HideToolbar = true, AlwaysExpanded = true)]
        private InfoId[] _sheets;

        //constructor
        public PageId(string parentSheet, InfoId[] sheets)
        {
            _parentSheet = parentSheet;
            _sheets = sheets;
        }

        //public variable
        public string ParentSheet => _parentSheet;
        public InfoId[] Sheet => _sheets;
    }

    [Serializable]
    public class InfoId
    {
        [TableColumnWidth(100)] [ReadOnly] [SerializeField]
        private string _id;

        [ReadOnly] [SerializeField] private string _description;
        
        [SerializeField]
        public bool IsUsing;
    }
}