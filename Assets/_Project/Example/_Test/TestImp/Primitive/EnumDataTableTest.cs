using NUnit.Framework;

public class EnumDataTableTest : BasePrimitiveDataTableTest<EnumDataTable, EnumTableData>
{
    protected override string ValueString => "0";
    
    
    [TestCase(0, "0")]
    [TestCase(0, "Enum1")]
    public void _04_CheckValueIsEqual(int expectedIndex, string valueString)
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
        Assert.AreEqual(expectedIndex, (int)tableData.Value, $"TableData's value is not match.");
    }
}