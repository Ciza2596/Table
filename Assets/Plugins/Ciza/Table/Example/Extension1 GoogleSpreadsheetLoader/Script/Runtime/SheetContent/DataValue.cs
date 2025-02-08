using System;
using CizaTable;
using UnityEngine;

namespace GoogleSpreadsheetLoader
{
	[Serializable]
	public class DataValue : IDataValue
	{
		//private variable
		[SerializeField]
		private string _name;

		[SerializeField]
		private string _value;

		//constructor
		public DataValue() { }

		public DataValue(string name, string value)
		{
			_name  = name;
			_value = value;
		}

		//public variable
		public string Name        => _name;
		public string ValueString => _value;
	}
}
