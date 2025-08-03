using CizaTable;

public class TableModuleConfigExample : BaseTableModuleConfig
{
	public TableModuleConfigExample(IAssetProvider assetProvider) : base(assetProvider) =>
		AddTable(new PlayerTable());
}
