using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GoogleSpreadsheetLoader;
using UnityEngine;

namespace CizaTable
{
	public abstract class BaseTableModuleConfig : ITableModuleConfig
	{
		private IAssetProvider           _assetProvider;
		private Dictionary<Type, object> _tables;

		private Func<UniTask> _initializeTable;
		private List<string>  _tableNames = new List<string>();

		protected BaseTableModuleConfig(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider;

		public async void Install(Dictionary<Type, object> tables)
		{
			Debug.Log($"[{GetType().Name}::Install] Start Load Table.");

			_tables = tables;

			await ExecuteInstallTasks();
			ReleaseInitializeTable();

			ReleaseSheetContents();

			_assetProvider = null;
			_tables        = null;

			Debug.Log($"[{GetType().Name}::Install] Table is loaded.");
		}

		protected void AddTable<TTableData>(Table<TTableData> table) where TTableData : Table<TTableData>.TableData =>
			_initializeTable += async () => { await InitializeTable(table); };

		private async UniTask ExecuteInstallTasks()
		{
			if (_initializeTable == null)
				return;

			var installTasks = new List<UniTask>();
			foreach (var invocation in _initializeTable.GetInvocationList())
				installTasks.Add(((Func<UniTask>)invocation).Invoke());

			await UniTask.WhenAll(installTasks);
		}

		private void ReleaseInitializeTable() =>
			_initializeTable = null;

		private async UniTask InitializeTable<TTableData>(Table<TTableData> table) where TTableData : Table<TTableData>.TableData
		{
			var tableName = table.Name;
			_tableNames.Add(tableName);

			var sheetContent = await _assetProvider.LoadAssetAsync<SheetContent>(tableName, default);
			var dataUnits    = sheetContent.DataUnits.ToArray();
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
