using CizaTable;

public class StringTable : Table<StringTable.Data>
{
    public class Data : TableData
    {
        public Data(string key) : base(key) { }
        public string Value { get; private set; }
    }
}