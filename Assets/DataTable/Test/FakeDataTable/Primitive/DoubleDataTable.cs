using DataTable;

public class DoubleDataTable: BaseDataTable<DoubleTableData>
{
}

public class DoubleTableData : BaseTableData
{
    public double Value { get; private set; }
}