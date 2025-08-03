using System;
using System.Collections.Generic;
using CizaTable;
using UnityEngine;

namespace GoogleSpreadsheetLoader
{
	[Serializable]
	public class SheetContent : ScriptableObject
	{
		[SerializeField]
		private DataMapList _dataMapList;

		[Header("已匯入資料(Raw)")]
		[Space]
		[SerializeField]
		private Array2D<string> _rawData;

		//public variable
		public IReadOnlyList<IDataUnit> DataUnits => _dataMapList.ToDataUnits();

		//public method
		public void UpdateContent(DataMapList dataMapList, string[,] rawData)
		{
			_dataMapList = dataMapList;
			_rawData = new Array2D<string>(rawData);
		}
	}
}