using NUnit.Framework;

public class StringTableTest : PrimitiveTableTest<StringTable, StringTable.Data>
{
    protected override string ValueString => "Hello";
    
    
    [TestCase("Hel lo", "Hel lo")]
    [TestCase("1 23 ", "1 23 ")]
    public void _04_CheckValueIsEqual(string expected, string valueString)
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
        Assert.AreEqual(expected, tableData.Value, $"TableData's value is not match.");
    }
}