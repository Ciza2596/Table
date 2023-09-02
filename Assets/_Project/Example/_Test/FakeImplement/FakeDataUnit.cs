using System;
using CizaTable;


[Serializable]
public class FakeDataUnit : IDataUnit
{
    
    //constructor
    public FakeDataUnit(string key, IDataValue[] dataValues)
    {
        Key = key;
        DataValues = dataValues;
    }


    //public variable
    public string Key { get; }
    public IDataValue[] DataValues { get; }
}