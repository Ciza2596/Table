using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable.Editor.MapListVisual
{
	public abstract class BMapListVE : ListVE
	{
		protected virtual string KeyPath => "_key";
		protected virtual string ValuePath => "_value";
		
		// PUBLIC VARIABLE: ---------------------------------------------------------------------

		public override bool IsAllowDisable => true;
		
		public override string GetItemTitle(int itemIndex, SerializedProperty itemProperty) => 
			itemProperty.FindPropertyRelative(KeyPath).GetValue<string>();

		// CONSTRUCTOR: ---------------------------------------------------------------------------

		[Preserve]
		protected BMapListVE(SerializedProperty listProperty, bool isAutoRefresh) : base(listProperty, isAutoRefresh) { }

		// PROTECT METHOD: --------------------------------------------------------------------

		protected override SerializedProperty CreateItemsProperty() =>
			ListProperty.FindPropertyRelative("_maps");

		protected abstract override ItemVE CreateItemVE(SerializedProperty itemProperty);
	}
}