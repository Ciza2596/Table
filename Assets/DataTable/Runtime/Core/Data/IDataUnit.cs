
namespace DataTable
{
    public interface IDataUnit
    {
        public string Key { get; }
        public IDataValue[] DataValues { get; }
    }
}