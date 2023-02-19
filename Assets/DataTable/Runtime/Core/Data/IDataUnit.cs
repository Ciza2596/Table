using System.Collections.Generic;

namespace DataTable
{
    public interface IDataUnit
    {
        public string Key { get; }
        public IReadOnlyList<IDataValue> DataValues { get; }
    }
}