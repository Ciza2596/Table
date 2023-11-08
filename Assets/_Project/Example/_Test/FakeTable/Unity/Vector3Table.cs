using CizaTable;
using UnityEngine;

public class Vector3Table: Table<Vector3Table.Data>
{
	public override string Name => "Vector3Table";
	
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public Vector3 Value { get; private set; }
	}
}
