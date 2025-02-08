using System;
using System.Collections.Generic;
using CizaTable;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GoogleSpreadsheetLoader
{
	[Serializable]
	public class SheetContent : SerializedScriptableObject
	{
		//private variable
		[TableList]
		[SerializeField]
		private DataUnit[] _dataUnits;

		[Header("已匯入資料(Raw)")]
		[OdinSerialize]
		private string[,] _rawData;

		//public variable
		public IReadOnlyList<IDataUnit> DataUnits => _dataUnits;

		//public method
		public void UpdateContent(DataUnit[] dataUnits, string[,] rawData)
		{
			_dataUnits = dataUnits;
			_rawData   = rawData;
		}
	}
}
