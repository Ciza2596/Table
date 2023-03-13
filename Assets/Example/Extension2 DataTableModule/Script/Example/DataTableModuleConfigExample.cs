using CizaAddressablesModule;
using CizaDataTable;

public class DataTableModuleConfigExample : BaseDataTableModuleConfig
{
    public DataTableModuleConfigExample(AddressablesModule addressablesModule) : base(addressablesModule)
    {
        AddDataTable(new PlayerDataTable());
    }
}