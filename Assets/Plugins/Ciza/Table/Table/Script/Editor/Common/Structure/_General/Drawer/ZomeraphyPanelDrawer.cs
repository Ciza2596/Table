using UnityEditor;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	[CustomPropertyDrawer(typeof(IZomeraphyPanel), true)]
	public class ZomeraphyPanelDrawer : PropertyDrawer
	{
		// PUBLIC METHOD: ----------------------------------------------------------------------
		
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var container = new BBoxVE.PropertyContentVE(property) { style = { paddingRight = 5 } };

			if (property.CheckHasAttribute<FlattenAttribute>(out _))
			{
				container.Refresh();
				return container;
			}

			var boxVE = new BoxVE(property);
			container.style.paddingRight = 0;
			boxVE.Initialize(property.displayName, container);
			return boxVE;
		}
	}
}