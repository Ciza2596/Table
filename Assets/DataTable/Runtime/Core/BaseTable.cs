
namespace DataTable
{
    public abstract class BaseTable<TTableData> where TTableData: BaseTableData, new()
    {
        //public variable
        public bool IsInitialized { get; private set; }
        
        //public method
        public void Initialize()
        {
            
        }

        public bool TryGetTableData(string key, out TTableData tableData)
        {
            tableData = null;
            return true;
        }
    }
}
