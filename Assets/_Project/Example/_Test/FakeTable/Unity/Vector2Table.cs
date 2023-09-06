using CizaTable;
using UnityEngine;

public class Vector2Table : Table<Vector2Table.Data>
{
	public class Data : TableData
	{
		public Data(string key) : base(key) { }

		protected string kl { get; private set; }

		protected Vector2 Value { get; private set; }

		public Vector2 OutputValue => Value;
	}
}
