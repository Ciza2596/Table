using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace CizaDataTable
{
	public abstract class DataTable<TTableData> where TTableData : TableData
	{
		//private variable
		private const string SPACE_TAG        = " ";
		private const string VECTOR_SPLIT_TAG = ",";
		private const string FLOAT_TAG        = "f";

		private static readonly CultureInfo                    _cultureInfo = CultureInfo.InvariantCulture;
		private                 Dictionary<string, TTableData> _dataTableMap;

		//constructor
		protected DataTable() => Name = GetType().Name;

		//public variable
		public string Name { get; }

		public bool IsInitialized => _dataTableMap != null;

		//public method
		public void Initialize(IDataUnit[] dataUnits)
		{
			Assert.IsNotNull(dataUnits, $"[{GetType().Name}::Initialize] SheetContent is null.");

			_dataTableMap = new Dictionary<string, TTableData>();
			Parser(dataUnits);
		}

		public void Release()
		{
			if (!IsInitialized)
				return;

			_dataTableMap.Clear();
			_dataTableMap = null;
		}

		public bool TryGetKeys(out string[] keys)
		{
			keys = _dataTableMap.Keys.ToArray();

			return keys != null && keys.Length > 0;
		}

		public bool TryGetKeyValuePair(out KeyValuePair<string, TTableData>[] keyValuePairs)
		{
			keyValuePairs = _dataTableMap.ToArray();
			return keyValuePairs != null && keyValuePairs.Length > 0;
		}

		public bool TryGetTableData(string key, out TTableData tableData)
		{
			var hasValue = _dataTableMap.TryGetValue(key, out tableData);
			return hasValue;
		}

		public bool TryGetTableData(Predicate<TTableData> match, out TTableData tableData)
		{
			var tableDataList = _dataTableMap.Values.ToList();
			tableData = tableDataList.Find(match);

			return tableData != null;
		}

		public bool TryGetTableDatas(out TTableData[] tableDatas)
		{
			tableDatas = _dataTableMap.Values.ToArray();
			return tableDatas != null && tableDatas.Length > 0;
		}

		public bool TryGetTableDatas(Predicate<TTableData> match, out TTableData[] tableDatas)
		{
			var tableDataList = _dataTableMap.Values.ToList();
			tableDatas = tableDataList.FindAll(match).ToArray();

			return tableDatas != null && tableDatas.Length > 0;
		}

		//private method
		private void AddTableData(string key, TTableData tableData)
		{
			if (_dataTableMap.ContainsKey(key))
			{
				Debug.Log($"[{GetType().Name}::AddTableData] Already add key: {key}.");
				return;
			}

			_dataTableMap.Add(key, tableData);
		}

		private void Parser(IDataUnit[] dataUnits)
		{
			var tableDataPropertyInfoMap = CreateTableDataPropertyInfoMap();

			foreach (var dataUnit in dataUnits)
			{
				var key       = dataUnit.Key;
				var tableData = Activator.CreateInstance(typeof(TTableData), key) as TTableData;

				var dataValues = dataUnit.DataValues;

				foreach (var dataValue in dataValues)
				{
					var name = dataValue.Name;
					var hasTableDataPropertyInfo =
						tableDataPropertyInfoMap.TryGetValue(name, out var tableDataPropertyInfo);

					if (!hasTableDataPropertyInfo)
					{
						Debug.LogWarning($"[{GetType().Name}::Parser] TableData hasnt property: {name}.");
						continue;
					}

					var value = dataValue.ValueString;
					SetValue(tableDataPropertyInfo, value, tableData);
				}

				AddTableData(key, tableData);
			}
		}

		private Dictionary<string, PropertyInfo> CreateTableDataPropertyInfoMap()
		{
			var bindingFlags  = BindingFlags.Instance | BindingFlags.Public;
			var propertyInfos = typeof(TTableData).GetProperties(bindingFlags);

			var propertyInfoMap = new Dictionary<string, PropertyInfo>();

			foreach (var propertyInfo in propertyInfos)
			{
				if (propertyInfo.CanRead && propertyInfo.CanWrite)
				{
					var name = propertyInfo.Name;
					propertyInfoMap.Add(name, propertyInfo);
				}
			}

			return propertyInfoMap;
		}

		private void SetValue(PropertyInfo propertyInfo, string valueString, TTableData tableData)
		{
			var propertyType = propertyInfo.PropertyType;

			try
			{
				if (propertyType == typeof(string))
				{
					var value = string.IsNullOrWhiteSpace(valueString)
						            ? string.Empty
						            : valueString;

					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType.IsEnum)
				{
					if (string.IsNullOrWhiteSpace(valueString))
					{
						propertyInfo.SetValue(tableData, Enum.ToObject(propertyType, 0));
						return;
					}

					if (valueString.All(char.IsDigit))
					{
						propertyInfo.SetValue(tableData, Enum.ToObject(propertyType, int.Parse(valueString)));
						return;
					}

					propertyInfo.SetValue(tableData, Enum.Parse(propertyType, valueString));
					return;
				}

				valueString = GetValueStringWithNotSpaceAndLower(valueString);

				if (propertyType == typeof(bool))
				{
					var value = string.IsNullOrWhiteSpace(valueString)
						            ? false
						            : bool.Parse(valueString);

					propertyInfo.SetValue(tableData, value);
					return;
				}

				valueString = GetValueStringWithNotFloatTag(valueString);

				if (propertyType == typeof(double))
				{
					var value = string.IsNullOrWhiteSpace(valueString)
						            ? 0
						            : double.Parse(valueString, _cultureInfo);

					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(float))
				{
					var value = string.IsNullOrWhiteSpace(valueString)
						            ? 0
						            : float.Parse(valueString, _cultureInfo);

					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(int))
				{
					var value = string.IsNullOrWhiteSpace(valueString)
						            ? 0
						            : int.Parse(valueString, _cultureInfo);

					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(long))
				{
					var value = string.IsNullOrWhiteSpace(valueString)
						            ? 0
						            : long.Parse(valueString, _cultureInfo);

					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(Vector2))
				{
					var value = Vector2.zero;
					if (!string.IsNullOrWhiteSpace(valueString))
					{
						var valueStrings = valueString.Split(VECTOR_SPLIT_TAG);

						if (valueStrings.Length != 2)
						{
							Debug.LogError(
							               $"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key} TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString} - Vector2's valueString length isnt 2");
							return;
						}

						var x = float.Parse(valueStrings[0], _cultureInfo);
						var y = float.Parse(valueStrings[1], _cultureInfo);

						value = new Vector2(x, y);
					}

					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(Vector2Int))
				{
					var value = Vector2Int.zero;
					if (!string.IsNullOrWhiteSpace(valueString))
					{
						var valueStrings = valueString.Split(VECTOR_SPLIT_TAG);

						if (valueStrings.Length != 2)
						{
							Debug.LogError(
							               $"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key} TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString} - Vector2Int's valueString length isnt 2");
							return;
						}

						var x = int.Parse(valueStrings[0], _cultureInfo);
						var y = int.Parse(valueStrings[1], _cultureInfo);

						value = new Vector2Int(x, y);
					}

					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(Vector3))
				{
					var value = Vector3.zero;
					if (!string.IsNullOrWhiteSpace(valueString))
					{
						var valueStrings = valueString.Split(VECTOR_SPLIT_TAG);

						if (valueStrings.Length != 3)
						{
							Debug.LogError(
							               $"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key} TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString} - Vector3's valueString length isnt 3");
							return;
						}

						var x = float.Parse(valueStrings[0], _cultureInfo);
						var y = float.Parse(valueStrings[1], _cultureInfo);
						var z = float.Parse(valueStrings[2], _cultureInfo);

						value = new Vector3(x, y, z);
					}

					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(Vector3Int))
				{
					var value = Vector3Int.zero;
					if (!string.IsNullOrWhiteSpace(valueString))
					{
						var valueStrings = valueString.Split(VECTOR_SPLIT_TAG);

						if (valueStrings.Length != 3)
						{
							Debug.LogError(
							               $"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key} TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString} - Vector3's valueString length isnt 3");
							return;
						}

						var x = int.Parse(valueStrings[0], _cultureInfo);
						var y = int.Parse(valueStrings[1], _cultureInfo);
						var z = int.Parse(valueStrings[2], _cultureInfo);

						value = new Vector3Int(x, y, z);
					}

					propertyInfo.SetValue(tableData, value);
					return;
				}
			}
			catch
			{
				Debug.LogError(
				               $"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key}, TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString}");
			}
		}

		private string GetValueStringWithNotSpaceAndLower(string origin)
		{
			var valueString = origin.ToLower(_cultureInfo);

			if (valueString.Contains(SPACE_TAG))
				valueString = valueString.Replace(SPACE_TAG, null);


			return valueString;
		}

		private string GetValueStringWithNotFloatTag(string origin)
		{
			var valueString = origin.ToLower(_cultureInfo);

			if (valueString.Contains(FLOAT_TAG))
				valueString = valueString.Replace(FLOAT_TAG, null);

			return valueString;
		}
	}
}
