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
		// VARIABLE: -----------------------------------------------------------------------------

		protected readonly IAssetProvider _assetProvider;
		protected readonly List<string> _tableNames = new List<string>();

		protected Dictionary<Type, object> _tables;
		protected Func<AsyncToken, Awaitable> _initializeTable;

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		protected BaseTableModuleConfig(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider;

		// PUBLIC METHOD: ----------------------------------------------------------------------

		public virtual async Awaitable InstallAsync(Dictionary<Type, object> tables, AsyncToken asyncToken)
		{
			Debug.Log($"[{GetType().Name}::Install] Start to load table at time: {Time.time}.");

			_tables = tables;

			await ExecuteInstallTasks(asyncToken);
			ReleaseInitializeTable();

			ReleaseSheetContents();

			_tables = null;

			Debug.Log($"[{GetType().Name}::Install] Table is loaded at time: {Time.time}.");
		}

		// PROTECT METHOD: --------------------------------------------------------------------

		protected virtual void AddTable<TTableData>(Table<TTableData> table) where TTableData : TableData =>
			_initializeTable += async (asyncToken) => { await InitializeTable(table, asyncToken); };

		protected virtual async Awaitable ExecuteInstallTasks(AsyncToken asyncToken)
		{
			if (_initializeTable == null)
				return;

			var awaitables = new List<Awaitable>();
			foreach (var invocation in _initializeTable.GetInvocationList())
				awaitables.Add(((Func<AsyncToken, Awaitable>)invocation).Invoke(asyncToken));

			await Async.AllAsync(awaitables);
		}

		protected virtual void ReleaseInitializeTable() =>
			_initializeTable = null;

		protected virtual async Awaitable InitializeTable<TTableData>(Table<TTableData> table, AsyncToken asyncToken) where TTableData : TableData
		{
			var tableName = table.Name;
			_tableNames.Add(tableName);

			var sheetContent = await _assetProvider.LoadAssetAsync<SheetContent>(tableName, asyncToken);
			var dataUnits = sheetContent.DataUnits.ToArray();
			table.Initialize(dataUnits);

			_tables.Add(table.GetType(), table);
		}

		protected virtual void ReleaseSheetContents()
		{
			foreach (var tableName in _tableNames.ToArray())
				_assetProvider.UnloadAsset<SheetContent>(tableName);

			_tableNames.Clear();
		}
	}
}