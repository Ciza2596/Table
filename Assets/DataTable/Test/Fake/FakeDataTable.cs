using DataTable;

public class FakeDataTable: BaseDataTable<FakeTableData>
{
}

public class FakeTableData : BaseTableData
{
    public string CharacterKey { get; private set; }

}