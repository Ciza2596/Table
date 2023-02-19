using DataTable;

public class StringDataTable : BaseDataTable<StringTableData>
{
}

public class StringTableData : BaseTableData
{
    public string Value { get; private set; }
}