using CizaTable;

public class IntTable : BTable<IntTable.Data>
{
	public override string Name => "IntTable";

	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public int Value { get; private set; }
	}
}
