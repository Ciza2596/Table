using System;
using System.Collections.Generic;

namespace CizaDataTable
{
    public interface IDataTableModuleConfig
    {
        public void Install(Dictionary<Type, object> dataTables);
    }
}