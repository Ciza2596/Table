using CizaDataTable;
using UnityEngine;

public class Vector3DataTable: DataTable<Vector3TableData>
{
}

public class Vector3TableData : TableData
{
    public Vector3TableData(string key) : base(key) { }
    public Vector3 Value { get; private set; }
}