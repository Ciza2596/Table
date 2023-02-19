using NUnit.Framework;
using UnityEngine;

public class Vector2IntDataTableTest : BasePrimitiveDataTableTest<Vector2IntDataTable, Vector2IntTableData>
{
    protected override string ValueString => "1, 1";
    
    

    [TestCase("1, 2")]
    public void _04_CheckValueIsEqual( string valueString)
    {
        //arrange
        var expected = new Vector2Int(1, 2);
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