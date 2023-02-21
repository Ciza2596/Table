using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class SheetInfo
    {
        //private variable
        [TableColumnWidth(100)] [ReadOnly] [SerializeField]
        private string _id;

        [ReadOnly] [SerializeField] private string _description;

        [SerializeField] private bool _isUsing;

        //constructor
        public SheetInfo(string id, string description)
        {
            _id = id;
            _description = description;
        }


        //public variable
        public string Id => _id;
        public string Description => _description;
        public bool IsUsing => _isUsing;
    }
}