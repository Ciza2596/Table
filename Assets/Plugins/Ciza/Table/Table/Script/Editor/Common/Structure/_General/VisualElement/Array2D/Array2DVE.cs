using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public class Array2DVE : VisualElement, BBoxVE.IContent
	{
		// VARIABLE: -----------------------------------------------------------------------------

		[field: NonSerialized]
		protected readonly VisualElement _gridContainer = new VisualElement();

		[field: NonSerialized]
		protected Vector2IntField _sizeField;

		protected virtual string[] USSPaths => new[] { "Array2D" };

		protected virtual string[] Array2DClasses => new[] { "array2D" };

		protected virtual string SizePath => "_size";
		protected virtual string DataPath => "_data";
		protected virtual bool IsResizable { get; set; }

		[field: NonSerialized]
		protected virtual SerializedProperty Array2DProperty { get; }

		protected virtual SerializedProperty SizeProperty => Array2DProperty.FindPropertyRelative(SizePath);
		protected virtual SerializedProperty DataProperty => Array2DProperty.FindPropertyRelative(DataPath);

		// PUBLIC VARIABLE: ---------------------------------------------------------------------

		[field: NonSerialized]
		public bool IsInitialized { get; protected set; }


		// CONSTRUCTOR: ------------------------------------------------------------------------

		[Preserve]
		public Array2DVE(SerializedProperty property) => Array2DProperty = property;


		// PUBLIC METHOD: ----------------------------------------------------------------------

		public void Initialize(bool isResizable = false)
		{
			if (IsInitialized)
				return;
			IsInitialized = true;

			IsResizable = isResizable;

			foreach (var sheet in StyleSheetUtils.GetStyleSheets(USSPaths))
				styleSheets.Add(sheet);

			foreach (var c in Array2DClasses)
				AddToClassList(c);


			DerivedInitialize();
		}

		public VisualElement Body => this;

		public void Refresh()
		{
			var size = SizeProperty.vector2IntValue;
			_sizeField.value = size;

			_gridContainer.Clear();
			for (int y = 0; y < size.y; y++)
			{
				var row = new VisualElement { style = { flexDirection = FlexDirection.Row } };
				for (int x = 0; x < size.x; x++)
				{
					var index = y * size.x + x;
					var itemProperty = DataProperty.GetArrayElementAtIndex(index);
					var width = new Length(100f / size.x, LengthUnit.Percent);
					var propertyField = new PropertyField(itemProperty, string.Empty) { style = { width = width } };
					propertyField.BindProperty(itemProperty);
					row.Add(propertyField);
				}

				_gridContainer.Add(row);
			}
		}

		// PROTECT METHOD: --------------------------------------------------------------------

		protected virtual void DerivedInitialize()
		{
			_sizeField = new Vector2IntField(SizeProperty.displayName);
			_sizeField.AddToClassList(AlignLabel.UNITY_ALIGN_FIELD_CLASS);
			_sizeField.SetIsVisible(IsResizable);
			_sizeField.RegisterCallback<BlurEvent>(_ => Resize());
			Add(_sizeField);

			Add(_gridContainer);

			Refresh();
		}

		protected virtual void Resize()
		{
			var size = _sizeField.value;
			var array2D = Array2DProperty.GetValue<Array2D>();
			array2D.Resize(size);
			Array2DProperty.SetValue(array2D);
			SizeProperty.SetValue(size);
			Refresh();
		}
	}
}