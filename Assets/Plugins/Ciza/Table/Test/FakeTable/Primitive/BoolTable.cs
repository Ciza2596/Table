using CizaTable;

public class BoolTable : BTable<BoolTable.Data>
{
	public override string Name => "BoolTable";
	
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		
		public bool Value { get; private set; }
	}
}