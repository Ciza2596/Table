using NUnit.Framework;

public class IntDataTableTest : BasePrimitiveDataTableTest<IntDataTable, IntTableData>
{
    protected override string ValueString => "1";
    
    
    [TestCase(0, " 0 ")]
    [TestCase(10, "1  0")]
    public void _04_CheckValueIsEqual(int expected, string valueString)
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