using System;
using System.Collections.Generic;
using CizaAsync;
using UnityEngine;

namespace CizaTable
{
	public interface ITableModuleConfig
	{
		public Awaitable InstallAsync(Dictionary<Type, object> tables, AsyncToken asyncToken);
	}
}