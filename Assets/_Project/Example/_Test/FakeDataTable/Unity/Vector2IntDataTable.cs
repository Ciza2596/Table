using CizaDataTable;
using UnityEngine;

public class Vector2IntDataTable: DataTable<Vector2IntTableData>
{
}

public class Vector2IntTableData : TableData
{
    public Vector2IntTableData(string key) : base(key) { }
    public Vector2Int Value { get; private set; }
}