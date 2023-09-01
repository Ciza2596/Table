using CizaDataTable;
using NUnit.Framework;

public abstract class BasePrimitiveDataTableTest<TDataTable, TTableData>
    where TDataTable : DataTable<TTableData>, new() where TTableData : TableData

{
    //private variable
    protected const string DATA_UNIT_KEY_PREFIX = "Key";
    private const string PROPERTY_NAME = "Value";

    protected virtual string ValueString { get; }

    protected TDataTable _dataTable;

    [SetUp]
    public void SetUp() =>
        _dataTable = new TDataTable();

    [TearDown]
    public void DearDown() =>
        _dataTable.Release();


    [TestCase(2, 2)]
    public void _01_Initialize(int expectedCount, int dataUnitCount)
    {
        //arrange
        Check_DataTable_Doesnt_Be_Initialized();
        var fakeDataUnits = CreateFakeDataUnits(dataUnitCount, 1, ValueString);


        //act
        _dataTable.Initialize(fakeDataUnits);


        //assert
        Check_DataTable_Is_Initialized();
        Check_DataTable_DataUnitCount(expectedCount);
    }


    [Test]
    public void _02_Release()
    {
        //arrange
        Check_DataTable_Doesnt_Be_Initialized();

        var dataUnitCount = 3;
        var fakeDataUnits = CreateFakeDataUnits(dataUnitCount, 1, ValueString);
        _dataTable.Initialize(fakeDataUnits);

        Check_DataTable_Is_Initialized();
        Check_DataTable_DataUnitCount(dataUnitCount);


        //act
        _dataTable.Release();


        //assert
        Check_DataTable_Doesnt_Be_Initialized();
    }


    [Test]
    public void _03_TryGetTableData()
    {
        //arrange
        Check_DataTable_Doesnt_Be_Initialized();

        var dataUnitCount = 3;
        var fakeDataUnits = CreateFakeDataUnits(dataUnitCount, 1, ValueString);
        _dataTable.Initialize(fakeDataUnits);

        Check_DataTable_Is_Initialized();
        Check_DataTable_DataUnitCount(dataUnitCount);


        //act
        var key = DATA_UNIT_KEY_PREFIX + "0";
        _dataTable.TryGetTableData(key, out var tableData);


        //assert
        Assert.IsNotNull(tableData, $"key: {key} doesnt find tableData.");
    }


    //protected method
    protected void Check_DataTable_Is_Initialized()
        =>
            Assert.IsTrue(_dataTable.IsInitialized, "DataTable doesnt be initialized.");

    protected void Check_DataTable_Doesnt_Be_Initialized()
        =>
            Assert.IsFalse(_dataTable.IsInitialized, "DataTable is initialized.");


    protected void Check_DataTable_DataUnitCount(int expectedCount)
    {
        _dataTable.TryGetKeys(out var keys);
        var count = keys.Length;
        Assert.AreEqual(expectedCount, count,
            $"Count doesnt match. DataTableCount: {count}, ExpectedCount: {expectedCount}");
    }


    protected IDataUnit[] CreateFakeDataUnits(int dataUnitCount, int dataValueCount, string valueString)
    {
        var dataUnits = new IDataUnit[dataUnitCount];

        for (int i = 0; i < dataUnitCount; i++)
            dataUnits[i] = CreateFakeDataUnit(DATA_UNIT_KEY_PREFIX + i, dataValueCount, valueString);

        return dataUnits;
    }

    protected IDataUnit CreateFakeDataUnit(string dataUnitKey, int dataValueCount, string valueString)
    {
        var fakeDataValues = CreateFakeDataValues(dataValueCount, valueString);
        var fakeDataUnit = new FakeDataUnit(dataUnitKey, fakeDataValues);
        return fakeDataUnit;
    }

    protected IDataValue[] CreateFakeDataValues(int count, string valueString)
    {
        var dataValue = new IDataValue[count];

        for (var i = 0; i < count; i++)
            dataValue[i] = CreateFakeDataValue(PROPERTY_NAME, valueString);

        return dataValue;
    }

    protected IDataValue CreateFakeDataValue(string propertyName, string valueString) =>
        new FakeDataValue(propertyName, valueString);
}