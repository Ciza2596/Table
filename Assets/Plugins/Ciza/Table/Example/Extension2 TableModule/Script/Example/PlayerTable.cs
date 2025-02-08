using CizaTable;

public class PlayerTable : Table<PlayerTable.Data>
{
	public override string Name => "PlayerTable";
	
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
	}
}
