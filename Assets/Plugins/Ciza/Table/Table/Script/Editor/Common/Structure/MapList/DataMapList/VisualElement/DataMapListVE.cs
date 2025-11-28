using CizaTable.Editor.MapListVisual;
using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable.Editor
{
	public class DataMapListVE : MapListVE
	{
		[Preserve]
		public DataMapListVE(string keyLabel, string valueLabel, SerializedProperty listProperty, bool isAutoRefresh) : 
			base(keyLabel, valueLabel, listProperty, isAutoRefresh) { }

		protected override ItemVE CreateItemVE(SerializedProperty itemProperty)
		{
			var itemVE = new DataMapItemVE(_keyLabel, _valueLabel, this, itemProperty);
			itemVE.Initialize();
			return itemVE;
		}
	}
}
