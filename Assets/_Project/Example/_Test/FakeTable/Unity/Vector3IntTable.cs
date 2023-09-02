using CizaTable;
using UnityEngine;

public class Vector3IntTable: Table<Vector3IntTable.Data>
{
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public Vector3Int Value { get; private set; }
	}
}