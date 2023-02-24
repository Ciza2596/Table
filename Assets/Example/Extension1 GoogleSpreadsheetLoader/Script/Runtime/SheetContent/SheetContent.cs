using System;
using System.Collections.Generic;
using DataTable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSpreadsheetLoader
{
    [Serializable]
    public class SheetContent : SerializedScriptableObject
    {
        //private variable
        [TableList] [SerializeField] private DataUnit[] _dataUnits;

        [Header("已匯入資料(Raw)")] private string[,] _rawData;


        //public variable
        public IReadOnlyList<IDataUnit> DataUnits => _dataUnits;


        //public method
        public void Update(string sheetName, DataUnit[] dataUnits, string[,] rawData)
        {
            name = sheetName;
            _dataUnits = dataUnits;
            _rawData = rawData;
        }
    }
}