using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GoogleSpreadsheetLoader;
using UnityEngine;

namespace CizaDataTable
{
	public abstract class BaseDataTableModuleConfig : IDataTableModuleConfig
	{
		private IAssetProvider           _assetProvider;
		private Dictionary<Type, object> _dataTables;

		private Func<UniTask> _initializeDataTable;
		private List<string>  _dataTableNames = new List<string>();

		protected BaseDataTableModuleConfig(IAssetProvider assetProvider) =>
			_assetProvider = assetProvider;

		public async void Install(Dictionary<Type, object> dataTables)
		{
			Debug.Log("[BaseDataTablesModuleConfig::Install] Start Load DataTable.");

			_dataTables = dataTables;

			await ExecuteInstallTasks();
			ReleaseInitializeDataTable();

			ReleaseSheetContents();

			_assetProvider = null;
			_dataTables    = null;

			Debug.Log("[BaseDataTablesModuleConfig::Install] DataTable is loaded.");
		}

		protected void AddDataTable<TTableData>(DataTable<TTableData> dataTable) where TTableData : TableData =>
			_initializeDataTable += async () => { await InitializeDataTable(dataTable); };

		private async UniTask ExecuteInstallTasks()
		{
			if (_initializeDataTable == null)
				return;

			var installTasks = new List<UniTask>();
			foreach (var invocation in _initializeDataTable.GetInvocationList())
				installTasks.Add(((Func<UniTask>)invocation).Invoke());

			await UniTask.WhenAll(installTasks);
		}

		private void ReleaseInitializeDataTable() =>
			_initializeDataTable = null;

		private async UniTask InitializeDataTable<TTableData>(DataTable<TTableData> dataTable)
			where TTableData : TableData
		{
			var dataTableName = dataTable.Name;
			_dataTableNames.Add(dataTableName);

			var sheetContent = await _assetProvider.LoadAssetAsync<SheetContent>(dataTableName, default);
			var dataUnits    = sheetContent.DataUnits.ToArray();
			dataTable.Initialize(dataUnits);

			_dataTables.Add(dataTable.GetType(), dataTable);
		}

		private void ReleaseSheetContents()
		{
			var dataTableNames = _dataTableNames.ToArray();
			foreach (var dataTableName in dataTableNames)
				_assetProvider.UnloadAsset<SheetContent>(dataTableName);

			_dataTableNames.Clear();
			_dataTableNames = null;
		}
	}
}
