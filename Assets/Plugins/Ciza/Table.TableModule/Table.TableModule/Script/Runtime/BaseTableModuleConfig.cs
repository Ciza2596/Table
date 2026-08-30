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

		protected readonly List<string> _tableNames = new List<string>();
		protected Func<AsyncToken, Awaitable> _initializeTable;

		protected IAssetProvider _assetProvider;
		protected Dictionary<Type, BTable> _tables;

		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		protected BaseTableModuleConfig() { }

		// PUBLIC METHOD: ----------------------------------------------------------------------

		public virtual async Awaitable InstallAsync(IAssetProvider assetProvider, Dictionary<Type, BTable> tables, AsyncToken asyncToken)
		{
			Debug.Log($"[{GetType().Name}::InstallAsync] Start to load table at time: {Time.time}.");

			_assetProvider = assetProvider;
			_tables = tables;

			await ExecuteInstallTasks(asyncToken);
			ReleaseSheetContents();

			_tables = null;
			_assetProvider = null;

			Debug.Log($"[{GetType().Name}::InstallAsync] Table is loaded at time: {Time.time}.");
		}

		// PROTECT METHOD: --------------------------------------------------------------------

		protected virtual void AddTable<TTableData>(BTable<TTableData> table) where TTableData : TableData =>
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

		protected virtual async Awaitable InitializeTable<TTableData>(BTable<TTableData> table, AsyncToken asyncToken) where TTableData : TableData
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