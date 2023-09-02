using NUnit.Framework;

public class FloatTableTest : PrimitiveTableTest<FloatTable, FloatTable.Data>
{
    protected override string ValueString => "1.1";
    
    [TestCase(1.8f, "1   .8")]
    [TestCase(6.1f, " 6.1 ")]
    public void _04_CheckValueIsEqual(float expected, string valueString)
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