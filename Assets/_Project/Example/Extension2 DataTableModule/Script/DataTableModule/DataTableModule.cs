using System;
using System.Collections.Generic;

namespace CizaDataTable
{
    public class DataTableModule
    {
        //private variable
        private readonly Dictionary<Type, object> _dataTables = new Dictionary<Type, object>();

        
        //public constructor
        public DataTableModule(IDataTableModuleConfig dataTableModuleConfig) =>
            dataTableModuleConfig.Install(_dataTables);
        


        //public method
        public bool TryGetKeys<TDataTable, TTableData>(out string[] keys) where TDataTable : BaseDataTable<TTableData> where TTableData : BaseTableData
        {
            keys = null;

            var hasDataTable = TryGetDataTable<TDataTable>(out var dataTable);

            if (!hasDataTable)
                return false;

            return dataTable.TryGetKeys(out keys);
        }

        public bool TryGetTableDatas<TDataTable, TTableData>(out TTableData[] tableDatas)
            where TDataTable : BaseDataTable<TTableData> where TTableData : BaseTableData
        {
            tableDatas = null;

            var hasDataTable = TryGetDataTable<TDataTable>(out var dataTable);

            if (!hasDataTable)
                return false;

            return dataTable.TryGetTableDatas(out tableDatas);
        }

        public bool TryGetKeyValuePair<TDataTable, TTableData>(out KeyValuePair<string, TTableData>[] keyValuePairs)where TDataTable : BaseDataTable<TTableData> where TTableData : BaseTableData
        {
            keyValuePairs = null;

            var hasDataTable = TryGetDataTable<TDataTable>(out var dataTable);

            if (!hasDataTable)
                return false;

            return dataTable.TryGetKeyValuePair(out keyValuePairs);
        }

        public bool TryGetTableData<TDataTable, TTableData>(string key, out TTableData tableData)
            where TDataTable : BaseDataTable<TTableData> where TTableData : BaseTableData
        {
            tableData = null;

            var hasDataTable = TryGetDataTable<TDataTable>(out var dataTable);

            if (!hasDataTable)
                return false;

            return dataTable.TryGetTableData(key, out tableData);
        }

        public bool TryGetTableData<TDataTable, TTableData>(Predicate<TTableData> match, out TTableData tableData)
            where TDataTable : BaseDataTable<TTableData> where TTableData : BaseTableData
        {
            tableData = null;

            var hasDataTable = TryGetDataTable<TDataTable>(out var dataTable);

            if (!hasDataTable)
                return false;

            return dataTable.TryGetTableData(match, out tableData);
        }


        public bool TryGetTableDatas<TDataTable, TTableData>(Predicate<TTableData> match, out TTableData[] tableDatas)
            where TDataTable : BaseDataTable<TTableData> where TTableData : BaseTableData
        {
            tableDatas = null;

            var hasDataTable = TryGetDataTable<TDataTable>(out var dataTable);

            if (!hasDataTable)
                return false;

            return dataTable.TryGetTableDatas(match, out tableDatas);
        }

        //private method
        private bool TryGetDataTable<T>(out T dataTable)
        {
            dataTable = default;

            var type = typeof(T);
            if (!_dataTables.ContainsKey(type))
                return false;

            dataTable = (T)_dataTables[type];
            if (dataTable is null)
                return false;

            return true;
        }
    }
}