using DataTable;

public class LongDataTable: BaseDataTable<LongTableData>
{
}

public class LongTableData : BaseTableData
{
    public long Value { get; private set; }
}