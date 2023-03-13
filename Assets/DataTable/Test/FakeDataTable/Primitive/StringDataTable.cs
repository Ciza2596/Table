using CizaDataTable;

public class StringDataTable : BaseDataTable<StringTableData>
{
}

public class StringTableData : BaseTableData
{
    public StringTableData(string key) : base(key) { }
    public string Value { get; private set; }
}