using System;
using System.Collections.Generic;
using UnityEngine;

namespace CizaTable
{
	[Serializable]
	public class SheetContent : ScriptableObject
	{
		[SerializeField]
		private DataMapList _dataMapList;

#if UNITY_EDITOR
		[Header("已匯入資料(Raw)")]
		[Space]
		[SerializeField]
		private Array2D<string> _rawData;
#endif

		//public variable
		public IReadOnlyList<IDataUnit> DataUnits => _dataMapList.ToDataUnits();

		//public method
		public void UpdateContent(DataMapList dataMapList, string[,] rawData)
		{
			_dataMapList = dataMapList;
#if UNITY_EDITOR
			_rawData = new Array2D<string>(rawData);
#endif
		}
	}
}