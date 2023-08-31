using NUnit.Framework;

public class FloatDataTableTest : BasePrimitiveDataTableTest<FloatDataTable, FloatTableData>
{
    protected override string ValueString => "1.1";
    
    [TestCase(1.8f, "1   .8")]
    [TestCase(6.1f, " 6.1 ")]
    public void _04_CheckValueIsEqual(float expected, string valueString)
    {
        //arrange
        Check_DataTable_Doesnt_Be_Initialized();

        var dataUnitCount = 3;
        var fakeDataUnits = CreateFakeDataUnits(dataUnitCount, 1, valueString);
        _dataTable.Initialize(fakeDataUnits);

        Check_DataTable_Is_Initialized();
        Check_DataTable_DataUnitCount(dataUnitCount);


        //act
        var key = DATA_UNIT_KEY_PREFIX + "0";
        _dataTable.TryGetTableData(key, out var tableData);


        //assert
        Assert.AreEqual(expected, tableData.Value, $"TableData's value is not match.");
    }
}