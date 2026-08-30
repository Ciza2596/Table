using System;
using System.Collections.Generic;
using System.Linq;
using CizaAsync;
using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	public class TableModule
	{
		//private variable
		protected readonly Dictionary<Type, BTable> _tables = new Dictionary<Type, BTable>();

		protected readonly ITableModuleConfig _config;
		protected readonly IAssetProvider _assetProvider;

		public virtual bool IsInitializing { get; protected set; }
		public virtual bool IsInitialized { get; protected set; }

		//public constructor
		[Preserve]
		public TableModule(ITableModuleConfig config, IAssetProvider assetProvider)
		{
			_config = config;
			_assetProvider = assetProvider;
		}

		//public method
		public virtual async Awaitable InitializeAsync(AsyncToken asyncToken)
		{
			if (IsInitialized || IsInitializing)
				return;

			IsInitializing = true;
			await _config.InstallAsync(_assetProvider, _tables, asyncToken);
			IsInitializing = false;
			IsInitialized = true;
		}

		public virtual void Release()
		{
			if (!IsInitialized || IsInitializing)
				return;

			foreach (var table in _tables.Values.ToArray())
				table.Release();

			_tables.Clear();
			IsInitialized = false;
		}

		public virtual bool TryGetTable<T>(out T table) where T : BTable
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

		public virtual bool TryGetKeys<TTable, TTableData>(out string[] keys) where TTable : BTable<TTableData> where TTableData : TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				keys = null;
				return false;
			}

			return dataTable.TryGetKeys(out keys);
		}

		public virtual bool TryGetTableDatas<TTable, TTableData>(out TTableData[] tableDatas) where TTable : BTable<TTableData> where TTableData : TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				tableDatas = null;
				return false;
			}

			return dataTable.TryGetTableDatas(out tableDatas);
		}

		public virtual bool TryGetKeyValuePair<TTable, TTableData>(out KeyValuePair<string, TTableData>[] keyValuePairs) where TTable : BTable<TTableData> where TTableData : TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				keyValuePairs = null;
				return false;
			}

			return dataTable.TryGetKeyValuePair(out keyValuePairs);
		}

		public virtual bool TryGetTableData<TTable, TTableData>(string key, out TTableData tableData) where TTable : BTable<TTableData> where TTableData : TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				tableData = null;
				return false;
			}

			return dataTable.TryGetTableData(key, out tableData);
		}

		public virtual bool TryGetTableData<TTable, TTableData>(Predicate<TTableData> match, out TTableData tableData) where TTable : BTable<TTableData> where TTableData : TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				tableData = null;
				return false;
			}

			return dataTable.TryGetTableData(match, out tableData);
		}

		public virtual bool TryGetTableDatas<TTable, TTableData>(Predicate<TTableData> match, out TTableData[] tableDatas) where TTable : BTable<TTableData> where TTableData : TableData
		{
			if (!TryGetTable<TTable>(out var dataTable))
			{
				tableDatas = null;
				return false;
			}

			return dataTable.TryGetTableDatas(match, out tableDatas);
		}
	}
}