using System;
using System.Collections.Generic;

namespace DataTable
{
    public interface IDataTablesModuleConfig
    {
        public void Install(Dictionary<Type, object> dataTables);
    }
}