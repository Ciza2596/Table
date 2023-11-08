using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable.Example1
{
	public class PlayerTable : Table<PlayerTable.Data>
	{
		public override string Name => "PlayerTable";
		
		public class Data : TableData
		{
			[Preserve]
			public Data(string key) : base(key) { }

			protected float Hp1_1 { get; private set; }

			public float Mp1_1 { get; private set; }

			public Vector2 Position { get; private set; }
		}
		
	}
}
