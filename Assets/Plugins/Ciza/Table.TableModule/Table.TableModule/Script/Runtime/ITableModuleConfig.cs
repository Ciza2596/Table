using System;
using System.Collections.Generic;
using CizaAsync;
using UnityEngine;

namespace CizaTable
{
	public interface ITableModuleConfig
	{
		public Awaitable InstallAsync(IAssetProvider assetProvider, Dictionary<Type, BTable> tables, AsyncToken asyncToken);
	}
}