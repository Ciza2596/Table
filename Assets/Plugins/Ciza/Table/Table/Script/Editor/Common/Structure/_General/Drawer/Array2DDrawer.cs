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

		public override VisualElement CreatePropertyGUI(SerializedProperty property) =>
			base.CreatePropertyGUI(property);

		// PROTECT METHOD: --------------------------------------------------------------------

		protected sealed override VisualElement CreateHeadAdditional(SerializedProperty property, BoxVE root) => CreateArray2DSizeLabel(property, root);

		protected virtual Label CreateArray2DSizeLabel(SerializedProperty property, BoxVE root)
		{
			var sizeProperty = property.FindPropertyRelative("_size");
			var size = sizeProperty.vector2IntValue;
			var label = new Label(string.Format(SizeTextFormat, size.x, size.y)) { style = { paddingRight = 8 } };
			label.TrackPropertyValue(sizeProperty, sizeProperty_ =>
			{
				label.text = string.Format(SizeTextFormat, sizeProperty_.vector2IntValue.x, sizeProperty_.vector2IntValue.y);
				root.Content.Refresh();
			});
			return label;
		}

		protected override BBoxVE.IContent CreateBody(SerializedProperty property, BoxVE root)
		{
			var array2DVE = new Array2DVE(property);
			array2DVE.Initialize();
			return array2DVE;
		}
	}
}