using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
    [Serializable]
    public abstract class BDataMapList<TValue> : BMapList<BDataMapList<TValue>.Map, TValue> where TValue : IEnumerable<IDataValue>
    {
        // CONSTRUCTOR: ------------------------------------------------------------------------

        [Preserve]
        protected BDataMapList() { }
        
        public abstract IDataUnit[] ToDataUnits();
        
        protected override Map CreateMap(string key, TValue value) =>
            new Map(key, value);
        
        [Serializable]
        public class Map : BMap<TValue>
        {
            [SerializeField]
            protected string _key;

            [SerializeField]
            [OverrideDrawer]
            protected TValue _dataValues;

            public override string Key => _key;

            public override TValue Value => _dataValues;

            [Preserve]
            public Map() : this("Default", default) { }

            [Preserve]
            public Map(string key, TValue value) : base()
            {
                _key = key;
                _dataValues = value;
            }

            [Preserve]
            public Map(string key, bool isEnable, TValue value) : base(isEnable)
            {
                _key = key;
                _dataValues = value;
            }
        }
    }
}
