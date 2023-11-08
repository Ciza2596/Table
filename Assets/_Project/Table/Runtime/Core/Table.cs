using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace CizaTable
{
	public abstract class Table<TTableData> where TTableData : TableData
	{
		public enum KeyLetterTypes
		{
			Normal,
			ToLower,
			ToUpper
		}

		//private variable
		private const string _sheetPrefixTag = "'";
		private const string _spaceTag       = " ";
		private const string _vectorSplitTag = ",";
		private const string _floatTag       = "f";
		private const float  _colorMax       = 255;

		private static readonly CultureInfo                    _cultureInfo = CultureInfo.InvariantCulture;
		private readonly        Dictionary<string, TTableData> _dataMap     = new Dictionary<string, TTableData>();

		//constructor
		protected Table() : this(KeyLetterTypes.Normal) { }

		protected Table(KeyLetterTypes keyLetterType) =>
			KeyLetterType = keyLetterType;

		//public variable
		public KeyLetterTypes KeyLetterType { get; }

		public abstract string Name { get; }

		public bool IsInitialized { get; private set; }

		//public method
		public void Initialize(IDataUnit[] dataUnits)
		{
			if (IsInitialized)
				return;

			IsInitialized = true;

			Assert.IsNotNull(dataUnits, $"[{GetType().Name}::Initialize] SheetContent is null.");
			Parser(dataUnits);
		}

		public void Release()
		{
			if (!IsInitialized)
				return;

			_dataMap.Clear();
			IsInitialized = false;
		}

		public bool TryGetKeys(out string[] keys)
		{
			if (!IsInitialized)
			{
				keys = Array.Empty<string>();
				return false;
			}

			keys = _dataMap.Keys.ToArray();

			return keys != null && keys.Length > 0;
		}

		public bool TryGetKeys(Predicate<TTableData> match, out string[] keys)
		{
			if (!IsInitialized)
			{
				keys = Array.Empty<string>();
				return false;
			}

			var tableDataList = _dataMap.Values.ToList();
			var tableDatas    = tableDataList.FindAll(match).ToArray();

			var keyList = new List<string>();
			foreach (var tableData in tableDatas)
				keyList.Add(tableData.Key);

			keys = keyList.ToArray();
			return true;
		}

		public bool TryGetKeyValuePair(out KeyValuePair<string, TTableData>[] keyValuePairs)
		{
			if (!IsInitialized)
			{
				keyValuePairs = Array.Empty<KeyValuePair<string, TTableData>>();
				return false;
			}

			keyValuePairs = _dataMap.ToArray();
			return true;
		}

		public bool TryGetTableData(string key, out TTableData tableData)
		{
			if (!IsInitialized)
			{
				tableData = null;
				return false;
			}

			var hasValue = _dataMap.TryGetValue(GetKey(key), out tableData);
			return hasValue;
		}

		public bool TryGetTableData(Predicate<TTableData> match, out TTableData tableData)
		{
			if (!IsInitialized)
			{
				tableData = null;
				return false;
			}

			var tableDataList = _dataMap.Values.ToList();
			tableData = tableDataList.Find(match);

			return tableData != null;
		}

		public bool TryGetTableDatas(out TTableData[] tableDatas)
		{
			if (!IsInitialized)
			{
				tableDatas = Array.Empty<TTableData>();
				return false;
			}

			tableDatas = _dataMap.Values.ToArray();
			return true;
		}

		public bool TryGetTableDatas(Predicate<TTableData> match, out TTableData[] tableDatas)
		{
			if (!IsInitialized)
			{
				tableDatas = Array.Empty<TTableData>();
				return false;
			}

			var tableDataList = _dataMap.Values.ToList();
			tableDatas = tableDataList.FindAll(match).ToArray();

			return true;
		}

		//private method
		private void Parser(IDataUnit[] dataUnits)
		{
			var tableDataPropertyInfoMap = CreateTableDataPropertyInfoMap();
			foreach (var dataUnit in dataUnits)
			{
				var key = GetKey(dataUnit.Key);
				if (_dataMap.ContainsKey(key))
				{
					Debug.Log($"[{GetType().Name}::Parser] Already add key: {key}.");
					continue;
				}

				var tableData = Activator.CreateInstance(typeof(TTableData), key) as TTableData;

				var dataValues = dataUnit.DataValues;
				foreach (var dataValue in dataValues)
				{
					var name                     = dataValue.Name;
					var hasTableDataPropertyInfo = tableDataPropertyInfoMap.TryGetValue(name, out var tableDataPropertyInfo);

					if (!hasTableDataPropertyInfo)
					{
						Debug.LogWarning($"[{GetType().Name}::Parser] TableData hasnt property: {name}.");
						continue;
					}

					var value = dataValue.ValueString;
					SetValue(tableDataPropertyInfo, value, tableData);
				}

				_dataMap.Add(key, tableData);
			}
		}

		private Dictionary<string, PropertyInfo> CreateTableDataPropertyInfoMap()
		{
			var propertyInfoMap = new Dictionary<string, PropertyInfo>();

			var bindingFlags  = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			var propertyInfos = typeof(TTableData).GetProperties(bindingFlags);

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

			valueString = GetValueStringWithRemoveTag(valueString, _sheetPrefixTag);
			try
			{
				if (propertyType == typeof(string))
				{
					var value = string.IsNullOrWhiteSpace(valueString) ? string.Empty : valueString;
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

				valueString = GetValueStringWithRemoveTag(valueString, _spaceTag);
				if (propertyType == typeof(bool))
				{
					var value = string.IsNullOrWhiteSpace(valueString) ? false : bool.Parse(valueString);
					propertyInfo.SetValue(tableData, value);
					return;
				}

				valueString = GetValueStringWithRemoveTag(valueString, _floatTag);
				if (propertyType == typeof(double))
				{
					var value = string.IsNullOrWhiteSpace(valueString) ? 0 : double.Parse(valueString, _cultureInfo);
					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(float))
				{
					var value = string.IsNullOrWhiteSpace(valueString) ? 0 : float.Parse(valueString, _cultureInfo);
					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(int))
				{
					var value = string.IsNullOrWhiteSpace(valueString) ? 0 : int.Parse(valueString, _cultureInfo);
					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(long))
				{
					var value = string.IsNullOrWhiteSpace(valueString) ? 0 : long.Parse(valueString, _cultureInfo);
					propertyInfo.SetValue(tableData, value);
					return;
				}

				if (propertyType == typeof(Vector2))
				{
					var value = Vector2.zero;
					if (!string.IsNullOrWhiteSpace(valueString))
					{
						var valueStrings = valueString.Split(_vectorSplitTag);
						if (valueStrings.Length != 2)
						{
							Debug.LogError($"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key} TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString} - Vector2's valueString length isnt 2");
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
						var valueStrings = valueString.Split(_vectorSplitTag);
						if (valueStrings.Length != 2)
						{
							Debug.LogError($"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key} TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString} - Vector2Int's valueString length isnt 2");
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
						var valueStrings = valueString.Split(_vectorSplitTag);
						if (valueStrings.Length != 3)
						{
							Debug.LogError($"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key} TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString} - Vector3's valueString length isnt 3");
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
						var valueStrings = valueString.Split(_vectorSplitTag);
						if (valueStrings.Length != 3)
						{
							Debug.LogError($"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key} TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString} - Vector3's valueString length isnt 3");
							return;
						}

						var x = int.Parse(valueStrings[0], _cultureInfo);
						var y = int.Parse(valueStrings[1], _cultureInfo);
						var z = int.Parse(valueStrings[2], _cultureInfo);

						value = new Vector3Int(x, y, z);
					}

					propertyInfo.SetValue(tableData, value);
				}

				if (propertyType == typeof(Color))
				{
					var value = Color.white;
					if (!string.IsNullOrWhiteSpace(valueString))
					{
						var valueStrings = valueString.Split(_vectorSplitTag);
						if (valueStrings.Length != 4)
						{
							Debug.LogError($"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key} TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString} - Color's valueString length isnt 4");
							return;
						}

						var r = int.Parse(valueStrings[0], _cultureInfo);
						var g = int.Parse(valueStrings[1], _cultureInfo);
						var b = int.Parse(valueStrings[2], _cultureInfo);
						var a = int.Parse(valueStrings[3], _cultureInfo);

						value = new Color(r / _colorMax, g / _colorMax, b / _colorMax, a / _colorMax);
					}

					propertyInfo.SetValue(tableData, value);
				}
			}
			catch
			{
				Debug.LogError($"[{GetType().Name}::SetValue] TableDataKey: {tableData.Key}, TableData: {tableData.GetType().Name}, PropertyName: {propertyInfo.Name}, value: {valueString}");
			}
		}

		private string GetValueStringWithRemoveTag(string origin, string tag) =>
			origin.Contains(tag) ? origin.Replace(tag, null) : origin;

		string GetKey(string key)
		{
			if (string.IsNullOrEmpty(key))
				return string.Empty;

			switch (KeyLetterType)
			{
				case KeyLetterTypes.ToLower:
					return key.ToLower();

				case KeyLetterTypes.ToUpper:
					return key.ToUpper();
			}

			return key;
		}
	}
}
