using CizaTable.Editor;
using UnityEditor;
using UnityEngine.Scripting;

namespace GoogleSpreadsheetLoader.Editor
{
	public class SpreadsheetInfosVE : ListVE
	{
		[Preserve]
		public SpreadsheetInfosVE(SerializedProperty listProperty) : base(listProperty) { }
		
		protected override ItemVE CreateItem(SerializedProperty itemProperty)
		{
			var spreadsheetInfoVE = new SpreadsheetInfoVE(this, itemProperty);
			spreadsheetInfoVE.Initialize();
			return spreadsheetInfoVE;
		}
	}
}