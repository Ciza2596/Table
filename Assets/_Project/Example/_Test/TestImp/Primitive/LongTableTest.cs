using NUnit.Framework;

public class LongTableTest : PrimitiveTableTest<LongTable, LongTable.Data>
{
    protected override string ValueString => "100";
    
    
    [TestCase(1000, "1000")]
    [TestCase(99, "9 9 ")]
    public void _04_CheckValueIsEqual(long expected, string valueString)
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