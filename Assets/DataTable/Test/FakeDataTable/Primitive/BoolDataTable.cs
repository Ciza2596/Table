using DataTable;

public class BoolDataTable : BaseDataTable<BoolTableData>
{
}

public class BoolTableData : BaseTableData
{
    public bool BoolValue { get; private set; }
}