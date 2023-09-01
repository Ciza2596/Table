using CizaDataTable;

public class StringDataTable : DataTable<StringTableData>
{
}

public class StringTableData : TableData
{
    public StringTableData(string key) : base(key) { }
    public string Value { get; private set; }
}