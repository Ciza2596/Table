using System;
using CizaAsync;
using UnityEngine;

namespace CizaTable
{
	public static class TableModuleInstaller
	{
		public static Awaitable<TableModule> InstallAsync<TTableModuleConfig>(IAssetProvider assetProvider, bool isAutoInitialize = true, AsyncToken asyncToken = default) where TTableModuleConfig : BaseTableModuleConfig =>
			InstallAsync<TableModule, TTableModuleConfig>(assetProvider, isAutoInitialize, asyncToken);

		public static async Awaitable<TTableModule> InstallAsync<TTableModule, TTableModuleConfig>(IAssetProvider assetProvider, bool isAutoInitialize = true, AsyncToken asyncToken = default) where TTableModule : TableModule where TTableModuleConfig : BaseTableModuleConfig
		{
			var config = CreateObj<TTableModuleConfig>(typeof(TTableModuleConfig), assetProvider);
			var tableModule = CreateObj<TTableModule>(typeof(TTableModule), config);
			if (isAutoInitialize)
				await tableModule.InitializeAsync(asyncToken);
			return tableModule;

			TObj CreateObj<TObj>(Type type, params object[] arg) where TObj : class =>
				Activator.CreateInstance(type, arg) as TObj;
		}
	}
}