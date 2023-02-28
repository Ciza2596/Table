using DataTable;

public class DataTablesModuleConfigExample : BaseDataTablesModuleConfig
{
    public DataTablesModuleConfigExample(AddressablesModule.AddressablesModule addressablesModule) : base(addressablesModule)
    {
        AddDataTable(new PlayerDataTable());
    }
}