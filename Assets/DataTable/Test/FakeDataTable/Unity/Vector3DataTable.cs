using DataTable;
using UnityEngine;

public class Vector3DataTable: BaseDataTable<Vector3TableData>
{
}

public class Vector3TableData : BaseTableData
{
    public Vector3 Value { get; private set; }
}