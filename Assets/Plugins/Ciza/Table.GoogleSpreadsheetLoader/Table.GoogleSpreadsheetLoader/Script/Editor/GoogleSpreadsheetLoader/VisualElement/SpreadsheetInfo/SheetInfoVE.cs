using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public class SheetInfoVE : ItemVE
	{
		[NonSerialized]
		protected readonly Label _sheetIdLabel = new Label();

		[NonSerialized]
		protected readonly Label _sheetNameLabel = new Label();

		[NonSerialized]
		protected readonly Toggle _isUsingToggle = new Toggle();


		protected virtual string SheetIdPath => "_sheetId";
		protected virtual string SheetNamePath => "_sheetName";
		protected virtual string IsUsingPath => "_isUsing";

		protected virtual SerializedProperty SheetIdProperty => ItemProperty.FindPropertyRelative(SheetIdPath);
		protected virtual SerializedProperty SheetNameProperty => ItemProperty.FindPropertyRelative(SheetNamePath);
		protected virtual SerializedProperty IsUsingProperty => ItemProperty.FindPropertyRelative(IsUsingPath);

		[Preserve]
		public SheetInfoVE(ListVE root, SerializedProperty itemProperty) : base(root, itemProperty) { }

		public override void Initialize()
		{
			style.flexDirection = FlexDirection.Row;

			SetupLabel(_sheetIdLabel, 50);

			SetupLabel(_sheetNameLabel, 40);

			SetupIsUsingToggle();
			
			Refresh();
		}

		public override void Refresh()
		{
			base.Refresh();
			
			_sheetIdLabel.Unbind();
			_sheetIdLabel.BindProperty(SheetIdProperty);
			
			_sheetNameLabel.Unbind();
			_sheetNameLabel.BindProperty(SheetNameProperty);
			
			_isUsingToggle.Unbind();
			_isUsingToggle.BindProperty(IsUsingProperty);
		}

		protected virtual void SetupLabel(Label label, float widthPercentage)
		{
			label.style.width = Length.Percent(widthPercentage);
			label.style.flexGrow = 1;
			label.style.flexShrink = 1;
			label.style.overflow = Overflow.Hidden;
			label.style.opacity = 0.6f;
			label.SetMargin(0);
			label.SetPadding(8, 8, 2, 2);
			label.SetBorder(1, Color.black, SideKinds.BottomLeft);
			Add(label);
		}

		protected virtual void SetupIsUsingToggle()
		{
			_isUsingToggle.style.width = Length.Percent(10);
			_isUsingToggle.style.minWidth = 60;
			_isUsingToggle.style.flexGrow = 1;
			_isUsingToggle.style.flexShrink = 1;
			_isUsingToggle.SetMargin(0);
			_isUsingToggle.SetBorder(1, Color.black, SideKinds.NoTop);
			_isUsingToggle[0].style.justifyContent = Justify.Center;
			Add(_isUsingToggle);
		}
	}
}