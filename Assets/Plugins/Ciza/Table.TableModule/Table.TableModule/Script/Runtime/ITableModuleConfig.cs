using System;
using System.Collections.Generic;
using CizaUniTask;

namespace CizaTable
{
	public interface ITableModuleConfig
	{
		public UniTask Install(Dictionary<Type, object> tables);
	}
}
