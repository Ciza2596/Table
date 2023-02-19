using DataTable;

public class FloatDataTable: BaseDataTable<FloatTableData>
{
}

public class FloatTableData : BaseTableData
{
    public float Value { get; private set; }
}