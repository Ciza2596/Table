using NUnit.Framework;
using UnityEngine;

public class Vector3DataTableTest : BasePrimitiveDataTableTest<Vector3DataTable, Vector3TableData>
{
    protected override string ValueString => "116.15f, 93.2f, 0.56f";
    
    
    [TestCase("16.15f, 93.2f, 0.56f")]
    public void _04_CheckValueIsEqual(string valueString)
    {
        //arrange
        var expected = new Vector3(16.15f, 93.2f, 0.56f);
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