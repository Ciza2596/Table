using CizaTable;

public class LongTable : BTable<LongTable.Data>
{
	public override string Name => "LongTable";
	
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public long Value { get; private set; }
	}
}
