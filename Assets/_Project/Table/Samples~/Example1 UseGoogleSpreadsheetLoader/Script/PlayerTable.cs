using UnityEngine;

namespace CizaTable.Example1
{
	public class PlayerTable : Table<PlayerTable.Data>
	{
		public class Data : TableData
		{
			public Data(string key) : base(key) { }

			public float Hp1_1 { get; private set; }

			public float Mp1_1 { get; private set; }

			public Vector2 Position { get; private set; }
		}
	}
}
