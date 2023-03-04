using DataTable;

public class DataTableModuleConfigExample : BaseDataTableModuleConfig
{
    public DataTableModuleConfigExample(AddressablesModule.AddressablesModule addressablesModule) : base(addressablesModule)
    {
        AddDataTable(new PlayerDataTable());
    }
}