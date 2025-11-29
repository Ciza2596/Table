using CizaTable.Editor.MapListVisual;
using UnityEditor;

namespace CizaTable.Editor
{
	[CustomPropertyDrawer(typeof(BDataMapList<>), true)]
	public class DataMapListDrawer : BMapListDrawer
	{
		protected virtual string KeyLabel => "Key";
		protected virtual string ValueLabel => "Data Values";

		protected override BMapListVE CreateMapListVE(SerializedProperty property, BoxVE root)
		{
			var listVE = new DataMapListVE(property, false, KeyLabel, ValueLabel);
			listVE.Initialize();
			return listVE;
		}
	}
}