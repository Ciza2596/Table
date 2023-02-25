using DataTable;

public class DataTableModuleConfig : BaseDataTableModuleConfig
{
    public DataTableModuleConfig(AddressablesModule.AddressablesModule addressablesModule) : base(addressablesModule)
    {
        AddDataTable(new PlayerDataTable());
    }
}