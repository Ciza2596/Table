using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataTable.Implement
{
    [Serializable]
    public class DataUnit : IDataUnit
    {
        //private variable
        [SerializeField]
        private string _key;

        [SerializeField]
        private List<DataValue> _dataValues;

        
        //public variable
        public string Key => _key;
        public IReadOnlyList<IDataValue> DataValues => _dataValues.ToList<IDataValue>();
    }

    [Serializable]
    public class DataValue: IDataValue
    {
        //private variable
        [SerializeField]
        private string _name;

        [SerializeField] private string _value;


        //public variable
        public string Name => _name;
        public string Value => _value;
    }
}
