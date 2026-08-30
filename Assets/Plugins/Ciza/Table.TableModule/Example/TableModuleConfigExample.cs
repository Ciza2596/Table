using CizaTable;
using UnityEngine.Scripting;

public class TableModuleConfigExample : BaseTableModuleConfig
{
	[Preserve]
	public TableModuleConfigExample() : base() =>
		AddTable(new PlayerTable());
}