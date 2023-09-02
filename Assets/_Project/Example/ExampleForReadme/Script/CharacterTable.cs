using CizaTable;
using UnityEngine;

public class CharacterTable : Table<CharacterTable.Data>
{
	public class Data : TableData
	{
		public Data(string key) : base(key) { }

		public float Hp { get; private set; }

		public Vector2 Position { get; private set; }
	}
}
