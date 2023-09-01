using CizaDataTable;

public class EnumDataTable: DataTable<EnumTableData>
{
}

public class EnumTableData : TableData
{
    public EnumTableData(string key) : base(key) { }
    public FakeEnum Value { get; private set; }
}

public enum FakeEnum
{
    Enum1,
    Enum2
}