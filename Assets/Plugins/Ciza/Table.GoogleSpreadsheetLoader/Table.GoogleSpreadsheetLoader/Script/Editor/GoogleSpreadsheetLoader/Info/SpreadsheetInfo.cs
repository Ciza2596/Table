using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CizaTable.Editor
{
	[Serializable]
	public class SpreadsheetInfo : IZomeraphyPanel
	{
		//private variable

		[SerializeField]
		private string _spreadsheetId;

		[SerializeField]
		private string _sheetContentPath = "Assets/Table";

		[Space]
		[SerializeField]
		private string _spreadsheetName;

		[Space]
		[SerializeField]
		private List<SheetInfo> _sheetInfos;

		[HideInInspector]
		[SerializeField]
		private string _id;

		//public variable

		public string SpreadsheetName => _spreadsheetName;

		public IReadOnlyList<SheetInfo> SheetInfos => _sheetInfos;

		public string SheetContentPath => _sheetContentPath;
		public string SpreadsheetId => _spreadsheetId;

		//public method
		public string GetId()
		{
			if (string.IsNullOrWhiteSpace(_id))
				_id = Guid.NewGuid().ToString();

			return _id;
		}

		public SheetInfo FindSheetInfo(string sheetId)
		{
			var sheetInfo = _sheetInfos.Find(sheetInfo => sheetInfo.SheetId == sheetId);
			return sheetInfo;
		}

		public SheetInfo CreateSheetInfo(string sheetId)
		{
			var sheetInfo = new SheetInfo(sheetId);
			_sheetInfos.Add(sheetInfo);

			return sheetInfo;
		}

		public virtual void RemoveSheetInfo(int removeCount)
		{
			var count = _sheetInfos.Count;
			if (removeCount <= count)
				_sheetInfos.RemoveRange(count - removeCount, removeCount);
			else
				_sheetInfos.Clear();
		}

		public void OrderByIsUsing() => _sheetInfos = _sheetInfos.OrderByDescending(sheetInfo => sheetInfo.IsUsing).ToList();

		public void SetSpreadSheetName(string spreadSheetName) => _spreadsheetName = spreadSheetName;
	}
}