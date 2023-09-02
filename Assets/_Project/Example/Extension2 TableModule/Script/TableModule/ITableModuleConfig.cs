using System;
using System.Collections.Generic;

namespace CizaTable
{
    public interface ITableModuleConfig
    {
        public void Install(Dictionary<Type, object> tables);
    }
}