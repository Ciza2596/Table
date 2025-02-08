using CizaTable;
using UnityEngine;

public class ColorTable : Table<ColorTable.Data>
{
	public override string Name => "ColorTable";
	
	public class Data : TableData
	{
		public Data(string key) : base(key) { }
		public Color Value { get; private set; }
	}
}
