using NUnit.Framework;
using UnityEngine;

public class ColorTableTest : PrimitiveTableTest<ColorTable, ColorTable.Data>
{
	protected override string ValueString => "255, 255, 255, 255";


	[TestCase("255, 255, 255, 255")]
	public void _04_CheckValueIsEqual(string valueString)
	{
		//arrange
		var expected = new Color(1, 1, 1, 1);
		Check_Table_Doesnt_Be_Initialized();

		var dataUnitCount = 1;
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
