using CizaDataTable;

public class PlayerDataTable : BaseDataTable<PlayerTableData>
{
}

public class PlayerTableData : BaseTableData
{
    public PlayerTableData(string key) : base(key)
    {
    }
}
