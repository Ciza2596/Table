using DataTable;
using UnityEngine;

public class Vector2DataTable: BaseDataTable<Vector2TableData>
{
}

public class Vector2TableData : BaseTableData
{
    public Vector2 Value { get; private set; }
}