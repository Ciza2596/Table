using System;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
	[Serializable]
	public class SheetInfo
	{
		[SerializeField]
		private string _sheetId;

		[SerializeField]
		private string _name;

		[SerializeField]
		private bool _isUsing;

		[HideInInspector]
		[SerializeField]
		private string _id;

		//constructor
		public SheetInfo(string sheetId)
		{
			_id      = Guid.NewGuid().ToString();
			_sheetId = sheetId;
		}

		//public variable
		public string Id => _id;

		public string SheetId => _sheetId;
		public string Name    => _name;
		public bool   IsUsing => _isUsing;

		//public method
		public void SetName(string name) =>
			_name = name;
	}
}
