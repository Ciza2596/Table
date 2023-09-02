using CizaTable;

public class LongTable : Table<LongTable.Data>
{
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public long Value { get; private set; }
	}
}
