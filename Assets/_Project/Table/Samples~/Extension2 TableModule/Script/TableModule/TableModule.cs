using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CizaTable
{
	public class TableModule
	{
		//private variable
		private readonly Dictionary<Type, object> _tables = new Dictionary<Type, object>();

		private ITableModuleConfig _tableModuleConfig;

		public bool IsInitializing { get; private set; }
		public bool IsInitialized  => _tableModuleConfig == null;

		//public constructor
		public TableModule(ITableModuleConfig tableModuleConfig) =>
			_tableModuleConfig = tableModuleConfig;

		//public method
		public async UniTask Initialize()
		{
			if (IsInitialized || IsInitializing)
				return;

			IsInitializing = true;
			await _tableModuleConfig.Install(_tables);
			IsInitializing = false;

			_tableModuleConfig = null;
		}

		public bool TryGetKeys<TTable, TTableData>(out string[] keys) where TTable : Table<TTableData> where TTableData : Table<TTableData>.TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				keys = null;
				return false;
			}

			return dataTable.TryGetKeys(out keys);
		}

		public bool TryGetTableDatas<TTable, TTableData>(out TTableData[] tableDatas) where TTable : Table<TTableData> where TTableData : Table<TTableData>.TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				tableDatas = null;
				return false;
			}

			return dataTable.TryGetTableDatas(out tableDatas);
		}

		public bool TryGetKeyValuePair<TTable, TTableData>(out KeyValuePair<string, TTableData>[] keyValuePairs) where TTable : Table<TTableData> where TTableData : Table<TTableData>.TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				keyValuePairs = null;
				return false;
			}

			return dataTable.TryGetKeyValuePair(out keyValuePairs);
		}

		public bool TryGetTableData<TTable, TTableData>(string key, out TTableData tableData) where TTable : Table<TTableData> where TTableData : Table<TTableData>.TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				tableData = null;
				return false;
			}

			return dataTable.TryGetTableData(key, out tableData);
		}

		public bool TryGetTableData<TTable, TTableData>(Predicate<TTableData> match, out TTableData tableData) where TTable : Table<TTableData> where TTableData : Table<TTableData>.TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				tableData = null;
				return false;
			}

			return dataTable.TryGetTableData(match, out tableData);
		}

		public bool TryGetTableDatas<TTable, TTableData>(Predicate<TTableData> match, out TTableData[] tableDatas) where TTable : Table<TTableData> where TTableData : Table<TTableData>.TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				tableDatas = null;
				return false;
			}

			return dataTable.TryGetTableDatas(match, out tableDatas);
		}

		//private method
		private bool TryGetTable<T>(out T table)
		{
			var type = typeof(T);
			if (!_tables.ContainsKey(type))
			{
				table = default;
				return false;
			}

			table = (T)_tables[type];
			return true;
		}
	}
}
