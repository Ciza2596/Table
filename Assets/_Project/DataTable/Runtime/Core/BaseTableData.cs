namespace CizaDataTable
{
    public abstract class BaseTableData
    {
        //constructor
        public BaseTableData(string key) => Key = key;
        
        //public variable
        public string Key { get; }
    }
}