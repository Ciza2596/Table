using CizaDataTable;

public class DoubleDataTable: DataTable<DoubleTableData>
{
}

public class DoubleTableData : TableData
{
    public DoubleTableData(string key) : base(key) { }
    public double Value { get; private set; }
}