using DataTable;

public class IntDataTable: BaseDataTable<IntTableData>
{
}

public class IntTableData : BaseTableData
{
    public int Value { get; private set; }
}