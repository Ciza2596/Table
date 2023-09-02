using CizaTable;
using NUnit.Framework;

public abstract class PrimitiveTableTest<TTable, TTableData> where TTable : Table<TTableData>, new() where TTableData : Table<TTableData>.TableData

{
	//private variable
	private const string _propertyName = "Value";

	protected const   string     _dataUnitKeyPrefix = "Key";
	protected virtual string     ValueString { get; }
	protected         TTable _table;

	[SetUp]
	public void SetUp() =>
		_table = new TTable();

	[TearDown]
	public void DearDown() =>
		_table.Release();

	[TestCase(2, 2)]
	public void _01_Initialize(int expectedCount, int dataUnitCount)
	{
		//arrange
		Check_Table_Doesnt_Be_Initialized();
		var fakeDataUnits = CreateFakeDataUnits(dataUnitCount, 1, ValueString);


		//act
		_table.Initialize(fakeDataUnits);


		//assert
		Check_Table_Is_Initialized();
		Check_Table_DataUnitCount(expectedCount);
	}

	[Test]
	public void _02_Release()
	{
		//arrange
		Check_Table_Doesnt_Be_Initialized();

		var dataUnitCount = 3;
		var fakeDataUnits = CreateFakeDataUnits(dataUnitCount, 1, ValueString);
		_table.Initialize(fakeDataUnits);

		Check_Table_Is_Initialized();
		Check_Table_DataUnitCount(dataUnitCount);


		//act
		_table.Release();


		//assert
		Check_Table_Doesnt_Be_Initialized();
	}

	[Test]
	public void _03_TryGetTableData()
	{
		//arrange
		Check_Table_Doesnt_Be_Initialized();

		var dataUnitCount = 3;
		var fakeDataUnits = CreateFakeDataUnits(dataUnitCount, 1, ValueString);
		_table.Initialize(fakeDataUnits);

		Check_Table_Is_Initialized();
		Check_Table_DataUnitCount(dataUnitCount);


		//act
		var key = _dataUnitKeyPrefix + "0";
		_table.TryGetTableData(key, out var tableData);


		//assert
		Assert.IsNotNull(tableData, $"key: {key} doesnt find tableData.");
	}

	//protected method
	protected void Check_Table_Is_Initialized() =>
			Assert.IsTrue(_table.IsInitialized, "Table should be initialized.");

	protected void Check_Table_Doesnt_Be_Initialized() =>
			Assert.IsFalse(_table.IsInitialized, "Table should be not initialized.");

	protected void Check_Table_DataUnitCount(int expectedCount)
	{
		_table.TryGetKeys(out var keys);
		var count = keys.Length;
		Assert.AreEqual(expectedCount, count, $"Count doesnt match. TableCount: {count}, ExpectedCount: {expectedCount}");
	}

	protected IDataUnit[] CreateFakeDataUnits(int dataUnitCount, int dataValueCount, string valueString)
	{
		var dataUnits = new IDataUnit[dataUnitCount];
		for (int i = 0; i < dataUnitCount; i++)
			dataUnits[i] = CreateFakeDataUnit(_dataUnitKeyPrefix + i, dataValueCount, valueString);

		return dataUnits;
	}

	protected IDataUnit CreateFakeDataUnit(string dataUnitKey, int dataValueCount, string valueString)
	{
		var fakeDataValues = CreateFakeDataValues(dataValueCount, valueString);
		var fakeDataUnit   = new FakeDataUnit(dataUnitKey, fakeDataValues);
		return fakeDataUnit;
	}

	protected IDataValue[] CreateFakeDataValues(int count, string valueString)
	{
		var dataValue = new IDataValue[count];
		for (var i = 0; i < count; i++)
			dataValue[i] = CreateFakeDataValue(_propertyName, valueString);

		return dataValue;
	}

	protected IDataValue CreateFakeDataValue(string propertyName, string valueString) =>
		new FakeDataValue(propertyName, valueString);
}
