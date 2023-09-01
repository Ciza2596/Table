using CizaDataTable;

public class DataTableModuleConfigExample : BaseDataTableModuleConfig
{
	public DataTableModuleConfigExample(IAssetProvider assetProvider) : base(assetProvider) =>
		AddDataTable(new PlayerDataTable());
}
