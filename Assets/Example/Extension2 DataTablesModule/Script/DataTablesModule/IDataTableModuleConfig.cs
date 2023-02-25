using System;
using System.Collections.Generic;

namespace DataTable
{
    public interface IDataTableModuleConfig
    {
        public void Install(Dictionary<Type, object> dataTables);
    }
}