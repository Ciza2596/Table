using System;
using System.Linq;
using CizaTable;
using UnityEngine;

namespace CizaTable.Example1
{
	public abstract class TableShower<TTable, TTableData> : MonoBehaviour where TTable : BTable<TTableData>, new() where TTableData : TableData
	{
		[SerializeField]
		private string _key;

		[SerializeField]
		private SheetContent _sheetContent;

		private readonly TTable _table = new TTable();

		private void Awake()
		{
			var dataUnits = _sheetContent.DataUnits.ToArray();
			_table.Initialize(dataUnits);
		}

		private void OnEnable()
		{
			if (_table.TryGetTableData(_key, out var tableData))
				LogTableData(tableData);
		}

		protected abstract void LogTableData(TTableData tableData);
	}
}
