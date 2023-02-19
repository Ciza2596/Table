using DataTable;

public class EnumDataTable: BaseDataTable<EnumTableData>
{
}

public class EnumTableData : BaseTableData
{
    public FakeEnum Value { get; private set; }
}

public enum FakeEnum
{
    Enum1,
    Enum2
}