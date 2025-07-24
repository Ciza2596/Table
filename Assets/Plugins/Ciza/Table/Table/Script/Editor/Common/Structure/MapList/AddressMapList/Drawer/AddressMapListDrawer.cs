using CizaTable.Editor.MapListVisual;
using UnityEditor;

namespace CizaTable.Editor
{
	[CustomPropertyDrawer(typeof(AddressMapList))]
	public class AddressMapListDrawer : MapListDrawer
	{
		protected override string KeyLabel => "DataId";
		protected override string ValueLabel => "Address";
	}
}