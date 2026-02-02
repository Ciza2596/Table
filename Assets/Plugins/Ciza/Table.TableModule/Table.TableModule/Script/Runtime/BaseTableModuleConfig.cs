using System;
using System.Collections.Generic;
using System.Linq;
using CizaAsync;
using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	public abstract class BaseTableModuleConfig : ITableModuleConfig
	{
		protected IAssetProvider _assetProvider;
		protected Dictionary<Type, object> _tables;

		protected Func<Awaitable> _initializeTable;
		protected List<string> _tableNames = new List<string>();

		[Preserve]
		protected BaseTableModuleConfig(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider;

		public async Awaitable Install(Dictionary<Type, object> tables)
		{
			Debug.Log($"[{GetType().Name}::Install] Start to load table at time: {Time.time}.");

			_tables = tables;

			await ExecuteInstallTasks();
			ReleaseInitializeTable();

			ReleaseSheetContents();

			_assetProvider = null;
			_tables = null;

			Debug.Log($"[{GetType().Name}::Install] Table is loaded at time: {Time.time}.");
		}

		protected void AddTable<TTableData>(Table<TTableData> table) where TTableData : TableData =>
			_initializeTable += async () => { await InitializeTable(table); };

		private async Awaitable ExecuteInstallTasks()
		{
			if (_initializeTable == null)
				return;

			var awaitables = new List<Awaitable>();
			foreach (var invocation in _initializeTable.GetInvocationList())
				awaitables.Add(((Func<Awaitable>)invocation).Invoke());

			await Async.AllAsync(awaitables);
		}

		private void ReleaseInitializeTable() =>
			_initializeTable = null;

		private async Awaitable InitializeTable<TTableData>(Table<TTableData> table) where TTableData : TableData
		{
			var tableName = table.Name;
			_tableNames.Add(tableName);

			var sheetContent = await _assetProvider.LoadAssetAsync<SheetContent>(tableName, AsyncToken.NONE);
			var dataUnits = sheetContent.DataUnits.ToArray();
			table.Initialize(dataUnits);

			_tables.Add(table.GetType(), table);
		}

		private void ReleaseSheetContents()
		{
			var tableNames = _tableNames.ToArray();
			foreach (var tableName in tableNames)
				_assetProvider.UnloadAsset<SheetContent>(tableName);

			_tableNames.Clear();
			_tableNames = null;
		}
	}
}