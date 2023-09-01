using CizaDataTable;

public class LongDataTable: DataTable<LongTableData>
{
}

public class LongTableData : TableData
{
    public LongTableData(string key) : base(key) { }
    public long Value { get; private set; }
}