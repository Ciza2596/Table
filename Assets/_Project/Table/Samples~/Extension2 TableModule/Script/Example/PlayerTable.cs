using CizaTable;

public class PlayerTable : Table<PlayerTable.Data>
{
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
	}
}
