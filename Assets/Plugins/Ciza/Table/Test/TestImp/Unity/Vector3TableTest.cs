using NUnit.Framework;
using UnityEngine;

public class Vector3TableTest : PrimitiveTableTest<Vector3Table, Vector3Table.Data>
{
    protected override string ValueString => "116.15f, 93.2f, 0.56f";
    
    
    [TestCase("16.15f, 93.2f, 0.56f")]
    public void _04_CheckValueIsEqual(string valueString)
    {
        //arrange
        var expected = new Vector3(16.15f, 93.2f, 0.56f);
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