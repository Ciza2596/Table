using NUnit.Framework;

public class BoolTableTest : PrimitiveTableTest<BoolTable, BoolTable.Data>
{
    protected override string ValueString => "True";

    [TestCase(true, "True")]
    [TestCase(false, "false")]
    public void _04_CheckValueIsEqual(bool expected, string valueString)
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