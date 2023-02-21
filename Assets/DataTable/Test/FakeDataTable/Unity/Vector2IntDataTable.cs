using DataTable;
using UnityEngine;

public class Vector2IntDataTable: BaseDataTable<Vector2IntTableData>
{
}

public class Vector2IntTableData : BaseTableData
{
    public Vector2IntTableData(string key) : base(key) { }
    public Vector2Int Value { get; private set; }
}