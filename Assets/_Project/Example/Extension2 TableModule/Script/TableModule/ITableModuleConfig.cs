using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CizaTable
{
	public interface ITableModuleConfig
	{
		public UniTask InstallAsync(Dictionary<Type, object> tables);
	}
}
