using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSheetLoader.Editor
{
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
}