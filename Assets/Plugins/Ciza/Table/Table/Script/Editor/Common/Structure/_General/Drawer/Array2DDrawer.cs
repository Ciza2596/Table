using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	[CustomPropertyDrawer(typeof(Array2D<>))]
	public class Array2DDrawer : BBoxDrawer
	{
		// VARIABLE: -----------------------------------------------------------------------------

		protected virtual string SizeTextFormat => "Size: {0} \u00d7 {1}";


		// PUBLIC METHOD: ----------------------------------------------------------------------

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			base.CreatePropertyGUI(property);
			return Root;
		}


		// PROTECT METHOD: --------------------------------------------------------------------

		protected sealed override VisualElement CreateHeadAdditional() => CreateArray2DSizeLabel();

		protected virtual Label CreateArray2DSizeLabel()
		{
			var sizeProperty = Property.FindPropertyRelative("_size");
			var size = sizeProperty.vector2IntValue;
			var label = new Label(string.Format(SizeTextFormat, size.x, size.y)) { style = { paddingRight = 8 } };
			label.TrackPropertyValue(sizeProperty, property => label.text = string.Format(SizeTextFormat, property.vector2IntValue.x, property.vector2IntValue.y));
			return label;
		}

		protected override BBoxVE.IContent CreateBody()
		{
			var array2DVE = new Array2DVE(Property);
			array2DVE.Initialize();
			return array2DVE;
		}
	}
}