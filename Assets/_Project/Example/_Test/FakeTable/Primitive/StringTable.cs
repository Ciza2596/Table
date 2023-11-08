using CizaTable;

public class StringTable : Table<StringTable.Data>
{
    public override string Name => "StringTable";
    
    public class Data : TableData
    {
        public Data(string key) : base(key) { }
        public string Value { get; private set; }
    }
}