using CizaTable;

public class FloatTable : Table<FloatTable.Data>
{
	public override string Name => "FloatTable";
	
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public float Value { get; private set; }
	}
}
