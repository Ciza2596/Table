using System;
using System.Collections.Generic;
using UnityEngine;

namespace CizaTable
{
	public interface ITableModuleConfig
	{
		public Awaitable Install(Dictionary<Type, object> tables);
	}
}
