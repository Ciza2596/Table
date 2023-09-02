using CizaTable;

public class DoubleTable : Table<DoubleTable.Data>
{
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public double Value { get; private set; }
	}
}
