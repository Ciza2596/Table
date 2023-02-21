using DataTable;

public class IntDataTable: BaseDataTable<IntTableData>
{
}

public class IntTableData : BaseTableData
{
    public IntTableData(string key) : base(key) { }
    public int Value { get; private set; }
}