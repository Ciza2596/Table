using System;
using CizaDataTable;


[Serializable]
public class FakeDataValue : IDataValue
{
    //constructor
    public FakeDataValue(string name, string value)
    {
        Name = name;
        ValueString = value;
    }


    //public variable
    public string Name { get; }
    public string ValueString { get; }
}