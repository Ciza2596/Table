using CizaTable;

public class FloatTable: Table<FloatTable.Data>
{
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public float Value { get; private set; }
	}
}
