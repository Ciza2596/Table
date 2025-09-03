using System;
using UnityEngine;

namespace CizaTable.Editor
{
	[Serializable]
	public class SheetInfo
	{
		[SerializeField]
		private string _sheetId;

		[SerializeField]
		private string _spreadsheetName;

		[SerializeField]
		private string _sheetName;

		[SerializeField]
		private bool _isUsing;

		[HideInInspector]
		[SerializeField]
		private string _id;

		//constructor
		public SheetInfo(string sheetId)
		{
			_id = Guid.NewGuid().ToString();
			_sheetId = sheetId;
		}

		//public variable
		public string Id => _id;

		public string SheetId => _sheetId;

		public string FullName => _spreadsheetName + " - " + _sheetName;

		public string SpreadsheetName => _spreadsheetName;
		public string SheetName => _sheetName;

		public bool IsUsing => _isUsing;

		//public method
		public void SetName(string spreadSheetName, string sheetName)
		{
			_spreadsheetName = spreadSheetName;
			_sheetName = sheetName;
		}
	}
}