using CizaTable;

public class IntTable : Table<IntTable.Data>
{
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public int Value { get; private set; }
	}
}
