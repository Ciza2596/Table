using CizaTable.Editor.MapListVisual;
using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable.Editor
{
	public class DataMapListVE : MapListVE
	{
		[Preserve]
		public DataMapListVE(string keyLabel, string valueLabel, SerializedProperty listProperty) : 
			base(keyLabel, valueLabel, listProperty) { }

		protected override ItemVE CreateItem(SerializedProperty itemProperty)
		{
			var itemVE = new DataMapItemVE(_keyLabel, _valueLabel, this, itemProperty);
			itemVE.Initialize();
			return itemVE;
		}
	}
}
