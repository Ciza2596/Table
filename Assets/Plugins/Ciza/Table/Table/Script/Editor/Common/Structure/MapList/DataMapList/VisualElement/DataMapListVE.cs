using CizaTable.Editor.MapListVisual;
using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable.Editor
{
	public class DataMapListVE : MapListVE
	{
		[Preserve]
		public DataMapListVE(SerializedProperty listProperty, bool isAutoRefresh, string keyLabel, string valueLabel) : base(listProperty, isAutoRefresh, keyLabel, valueLabel) { }

		protected override ItemVE CreateItemVE(SerializedProperty itemProperty)
		{
			var itemVE = new DataMapItemVE(this, itemProperty, _keyLabel, _valueLabel);
			itemVE.Initialize();
			return itemVE;
		}
	}
}