using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace CizaTable
{
	[Serializable]
	public class DataMapList : BDataMapList<DataValue[]>
	{
		[Preserve]
		public DataMapList() { }


		public override IDataUnit[] ToDataUnits()
		{
			var list = new List<IDataUnit>();

			foreach (var pair in KeyValuePairs)
				list.Add(new DataUnit(pair.Key, pair.Value));

			return list.ToArray();
		}
	}
}