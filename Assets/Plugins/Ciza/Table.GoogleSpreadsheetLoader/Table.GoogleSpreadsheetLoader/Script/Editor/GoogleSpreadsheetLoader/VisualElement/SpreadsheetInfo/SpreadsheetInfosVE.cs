using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable.Editor
{
	public class SpreadsheetInfosVE : ListVE
	{
		[Preserve]
		public SpreadsheetInfosVE(SerializedProperty listProperty, bool isAutoRefresh) : base(listProperty, isAutoRefresh) { }
		
		protected override ItemVE CreateItemVE(SerializedProperty itemProperty)
		{
			var spreadsheetInfoVE = new SpreadsheetInfoVE(this, itemProperty);
			spreadsheetInfoVE.Initialize();
			return spreadsheetInfoVE;
		}
	}
}