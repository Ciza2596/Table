using CizaDataTable;
using UnityEngine;

public class Vector3IntDataTable: DataTable<Vector3IntTableData>
{
}

public class Vector3IntTableData : TableData
{
    public Vector3IntTableData(string key) : base(key) { }
    public Vector3Int Value { get; private set; }
}