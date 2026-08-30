using CizaTable;

public class PlayerTable : BTable<PlayerTable.Data>
{
	public override string Name => "PlayerTable";
	
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
	}
}
