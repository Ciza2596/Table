using CizaDataTable;
using UnityEngine;

public class Vector3DataTable: BaseDataTable<Vector3TableData>
{
}

public class Vector3TableData : BaseTableData
{
    public Vector3TableData(string key) : base(key) { }
    public Vector3 Value { get; private set; }
}