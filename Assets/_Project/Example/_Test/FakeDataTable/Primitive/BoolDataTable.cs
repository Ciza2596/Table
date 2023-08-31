using CizaDataTable;

public class BoolDataTable : BaseDataTable<BoolTableData>
{
}

public class BoolTableData : BaseTableData
{
    public BoolTableData(string key) : base(key) { }
    public bool Value { get; private set; }
}