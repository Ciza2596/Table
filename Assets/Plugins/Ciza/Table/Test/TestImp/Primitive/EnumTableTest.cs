using NUnit.Framework;

public class EnumTableTest : PrimitiveTableTest<EnumTable, EnumTable.Data>
{
    protected override string ValueString => "0";
    
    
    [TestCase(0, "0")]
    [TestCase(0, "Enum1")]
    public void _04_CheckValueIsEqual(int expectedIndex, string valueString)
    {
        //arrange
        Check_Table_Doesnt_Be_Initialized();

        var dataUnitCount = 3;
        var fakeDataUnits = CreateFakeDataUnits(dataUnitCount, 1, valueString);
        _table.Initialize(fakeDataUnits);

        Check_Table_Is_Initialized();
        Check_Table_DataUnitCount(dataUnitCount);


        //act
        var key = _dataUnitKeyPrefix + "0";
        _table.TryGetTableData(key, out var tableData);


        //assert
        Assert.AreEqual(expectedIndex, (int)tableData.Value, $"TableData's value is not match.");
    }
}