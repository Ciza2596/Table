using CizaTable;

public class EnumTable : Table<EnumTable.Data>
{
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public FakeEnum Value { get; private set; }
	}

	public override string Name => "EnumTable";
}

public enum FakeEnum
{
	Enum1,
	Enum2
}
