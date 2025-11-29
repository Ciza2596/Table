using CizaTable.Editor.MapListVisual;
using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable.Editor
{
	public class DataMapItemVE : MapItemVE
	{
		protected override string ValuePath => "_dataValues";

		[Preserve]
		public DataMapItemVE(BMapListVE root, SerializedProperty itemProperty, string keyLabel, string valueLabel) : base(root, itemProperty, keyLabel, valueLabel) { }
	}
}