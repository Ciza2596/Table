using CizaDataTable;

public class DoubleDataTable: BaseDataTable<DoubleTableData>
{
}

public class DoubleTableData : BaseTableData
{
    public DoubleTableData(string key) : base(key) { }
    public double Value { get; private set; }
}