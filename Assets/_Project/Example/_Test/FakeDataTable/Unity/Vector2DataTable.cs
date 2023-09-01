using CizaDataTable;
using UnityEngine;

public class Vector2DataTable: DataTable<Vector2TableData>
{
}

public class Vector2TableData : TableData
{
    public Vector2TableData(string key) : base(key) { }
    public Vector2 Value { get; private set; }
}