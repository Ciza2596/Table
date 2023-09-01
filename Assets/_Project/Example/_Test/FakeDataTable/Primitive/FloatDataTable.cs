using CizaDataTable;

public class FloatDataTable: DataTable<FloatTableData>
{
}

public class FloatTableData : TableData
{
    public FloatTableData(string key) : base(key) { }
    public float Value { get; private set; }
}