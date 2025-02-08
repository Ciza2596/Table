using CizaTable;

public class DoubleTable : Table<DoubleTable.Data>
{
	public override string Name => "DoubleTable";
	
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public double Value { get; private set; }
	}
}
