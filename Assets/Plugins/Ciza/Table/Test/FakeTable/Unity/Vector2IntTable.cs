using CizaTable;
using UnityEngine;

public class Vector2IntTable : Table<Vector2IntTable.Data>
{
	public override string Name => "Vector2IntTable";

	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public Vector2Int Value { get; private set; }
	}
}
