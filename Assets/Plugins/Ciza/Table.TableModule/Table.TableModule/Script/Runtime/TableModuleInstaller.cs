using System;
using CizaAsync;
using UnityEngine;

namespace CizaTable
{
	public static class TableModuleInstaller
	{
		public static Awaitable<TableModule> InstallAsync(IAssetProvider assetProvider, ITableModuleConfig config, bool isAutoInitialize = true, AsyncToken asyncToken = default) =>
			InstallAsync<TableModule>(assetProvider, config, isAutoInitialize, asyncToken);

		public static async Awaitable<TTableModule> InstallAsync<TTableModule>(IAssetProvider assetProvider, ITableModuleConfig config, bool isAutoInitialize = true, AsyncToken asyncToken = default) where TTableModule : TableModule
		{
			var tableModule = CreateObj<TTableModule>(typeof(TTableModule), config, assetProvider);
			if (isAutoInitialize)
				await tableModule.InitializeAsync(asyncToken);
			return tableModule;

			TObj CreateObj<TObj>(Type type, params object[] arg) where TObj : class =>
				Activator.CreateInstance(type, arg) as TObj;
		}
	}
}