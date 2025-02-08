using NUnit.Framework;
using UnityEngine;

public class Vector3IntTableTest : PrimitiveTableTest<Vector3IntTable, Vector3IntTable.Data>
{
    protected override string ValueString => "156, 453, 0";
    
    
    [TestCase("16, 435, 9999")]
    public void _04_CheckValueIsEqual(string valueString)
    {
        //arrange
        var expected = new Vector3Int(16, 435, 9999);
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