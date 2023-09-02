using CizaDataTable;

public class PlayerDataTable : DataTable<PlayerTableData>
{
}

public class PlayerTableData : TableData
{
    public PlayerTableData(string key) : base(key)
    {
    }
}
