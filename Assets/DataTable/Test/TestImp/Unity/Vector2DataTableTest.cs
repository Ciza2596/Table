using NUnit.Framework;
using UnityEngine;

public class Vector2DataTableTest : BasePrimitiveDataTableTest<Vector2DataTable, Vector2TableData>
{
    protected override string ValueString => "1.05f, 156.3f";
    
    
    [TestCase("1.11f, 40.65f")]
    public void _04_CheckValueIsEqual(string valueString)
    {
        //arrange
        var expected = new Vector2(1.11f, 40.65f);
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