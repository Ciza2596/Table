using DataTable;
using UnityEngine;
public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var dataTablesModule = new DataTablesModule(new DataTableModuleConfig(new AddressablesModule.AddressablesModule()));

        dataTablesModule.TryGetValues<PlayerDataTable, PlayerTableData>(out var playerTableDatas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class PlayerDataTable: BaseDataTable<PlayerTableData>
{
    
}

public class PlayerTableData : BaseTableData
{
    public PlayerTableData(string key) : base(key)
    {
    }
}
