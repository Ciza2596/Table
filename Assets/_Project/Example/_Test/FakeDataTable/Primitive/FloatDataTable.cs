using CizaDataTable;

public class FloatDataTable: BaseDataTable<FloatTableData>
{
}

public class FloatTableData : BaseTableData
{
    public FloatTableData(string key) : base(key) { }
    public float Value { get; private set; }
}