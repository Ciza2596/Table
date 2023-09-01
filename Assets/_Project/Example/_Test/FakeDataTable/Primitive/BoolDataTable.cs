using CizaDataTable;

public class BoolDataTable : DataTable<BoolTableData>
{
}

public class BoolTableData : TableData
{
    public BoolTableData(string key) : base(key) { }
    public bool Value { get; private set; }
}