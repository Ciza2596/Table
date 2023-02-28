using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GoogleSpreadsheetLoader;
using UnityEngine;

namespace DataTable
{
    public abstract class BaseDataTablesModuleConfig : IDataTablesModuleConfig
    {
        private AddressablesModule.AddressablesModule _addressablesModule;
        private Dictionary<Type, object> _dataTables;

        private List<UniTask> _installTasks = new List<UniTask>();
        private List<string> _dataTableNames = new List<string>();

        protected BaseDataTablesModuleConfig(AddressablesModule.AddressablesModule addressablesModule) =>
            _addressablesModule = addressablesModule;


        public async void Install(Dictionary<Type, object> dataTables)
        {
            Debug.Log("[BaseDataTablesModuleConfig::Install] Start Load DataTable.");
            
            _dataTables = dataTables;

            await ExecuteInstallTasks();
            ReleaseInstallTasks();
            
            ReleaseSheetContents();

            _addressablesModule = null;
            _dataTables = null;
            
            Debug.Log("[BaseDataTablesModuleConfig::Install] DataTable is loaded.");
        }

        protected void AddDataTable<TTableData>(BaseDataTable<TTableData> dataTable) where TTableData : BaseTableData =>
            _installTasks.Add(InitializeDataTable(dataTable));
        
        
        private async UniTask ExecuteInstallTasks()
        {
            var installTasks = _installTasks.ToArray();
            await UniTask.WhenAll(installTasks);
        }
        
        private void ReleaseInstallTasks()
        {
            _installTasks.Clear();
            _installTasks = null;
        }

        private async UniTask InitializeDataTable<TTableData>(BaseDataTable<TTableData> dataTable)
            where TTableData : BaseTableData
        {
            var dataTableName = dataTable.Name;
            _dataTableNames.Add(dataTableName);
            
            var sheetContent = await _addressablesModule.GetAssetAsync<SheetContent>(dataTableName);
            var dataUnits = sheetContent.DataUnits.ToArray();
            dataTable.Initialize(dataUnits);

            _dataTables.Add(dataTable.GetType(), dataTable);
        }

        private void ReleaseSheetContents()
        {
            var dataTableNames = _dataTableNames.ToArray();
            _addressablesModule.ReleaseAssets(dataTableNames);

            _dataTableNames.Clear();
            _dataTableNames = null;
        }
    }
}