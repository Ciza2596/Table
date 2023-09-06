using UnityEngine.Scripting;

namespace CizaTable.Example1
{
	public class GoblinTable : Table<GoblinTable.Data>
	{
		public class Data : TableData
		{
			[Preserve]
			public Data(string key) : base(key) { }

			protected float Hp1_2 { get; private set; }

			protected float Mp1_2 { get; private set; }
		}
	}
}
