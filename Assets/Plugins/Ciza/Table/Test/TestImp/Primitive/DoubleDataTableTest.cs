using NUnit.Framework;

public class DoubaleTableTest : PrimitiveTableTest<DoubleTable, DoubleTable.Data>
{
    protected override string ValueString => "1.1";
    
    [TestCase(1.1, "1. 1")]
    [TestCase(8.0, "8 .0")]
    public void _04_CheckValueIsEqual(double expected, string valueString)
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