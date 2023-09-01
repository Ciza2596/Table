using CizaDataTable;

public class IntDataTable: DataTable<IntTableData>
{
}

public class IntTableData : TableData
{
    public IntTableData(string key) : base(key) { }
    public int Value { get; private set; }
}