using CizaDataTable;

public class LongDataTable: BaseDataTable<LongTableData>
{
}

public class LongTableData : BaseTableData
{
    public LongTableData(string key) : base(key) { }
    public long Value { get; private set; }
}