using System;
using System.Linq;
using DataTable;
using UnityEngine;

namespace GoogleSpreadsheetLoader
{
    [Serializable]
    public class DataUnit : IDataUnit
    {
        //private variable
        [SerializeField] private string _key;
        [SerializeField] private DataValue[] _dataValues;


        //constructor
        public DataUnit(string key, DataValue[] dataValues)
        {
            _key = key;
            _dataValues = dataValues;
        }


        //public variable
        public string Key => _key;
        public IDataValue[] DataValues => _dataValues.ToArray<IDataValue>();
    }
}