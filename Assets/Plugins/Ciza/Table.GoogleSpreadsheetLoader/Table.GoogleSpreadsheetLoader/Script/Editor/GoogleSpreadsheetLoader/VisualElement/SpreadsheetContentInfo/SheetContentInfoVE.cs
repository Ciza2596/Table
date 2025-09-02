using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public class SheetContentInfoVE : ItemVE
	{
		[NonSerialized]
		protected readonly ObjectField _sheetContentField = new ObjectField();

		[NonSerialized]
		protected readonly VisualElement _actionButtonsContainer = new VisualElement();
		
		[NonSerialized]
		protected Button _updateButton;
		
		protected virtual string SheetContentPath => "_sheetContent";
		
		protected virtual string IsBusyPath => "_isBusy";

		protected virtual SerializedProperty SheetContentProperty => ItemProperty.FindPropertyRelative(SheetContentPath);
		protected virtual SerializedProperty IsBusyProperty => ItemProperty.FindPropertyRelative(IsBusyPath);
		
		protected virtual GoogleSpreadsheetLoader GoogleSpreadsheetLoader => ItemProperty.serializedObject.targetObject as GoogleSpreadsheetLoader;


		[Preserve]
		public SheetContentInfoVE(SheetContentInfosVE root, SerializedProperty itemProperty) : base(root, itemProperty) { }

		public override void Initialize()
		{
			style.flexDirection = FlexDirection.Row;

			SetupScriptableObjectField();

			SetupActionButtons();
		}

		protected virtual void SetupScriptableObjectField()
		{
			_sheetContentField.objectType = typeof(SheetContent);
			_sheetContentField.allowSceneObjects = false;
			_sheetContentField.style.width = Length.Percent(60);
			_sheetContentField.style.flexGrow = 1;
			_sheetContentField.style.flexShrink = 1;
			_sheetContentField.SetMargin(0);
			_sheetContentField.SetPadding(5,5,2,2);
			_sheetContentField.SetBorder(1, Color.black, SideKinds.BottomLeft);
			_sheetContentField.SetValueWithoutNotify(SheetContentProperty.GetValue<SheetContent>());
			_sheetContentField.Q(className:"unity-object-field__selector").SetEnabled(false);
			_sheetContentField.AddManipulator(new ContextualMenuManipulator(null));

			Add(_sheetContentField);
		}
		
		protected virtual void SetupActionButtons()
		{
			_actionButtonsContainer.SetBorder(1, Color.black, SideKinds.NoTop);
			_actionButtonsContainer.style.flexGrow = 1;
			_actionButtonsContainer.style.flexShrink = 1;
			_actionButtonsContainer.style.flexDirection = FlexDirection.Row;
			_actionButtonsContainer.style.width = Length.Percent(40f);

			_updateButton = new Button(UpdateSheetContentInfo) { text = "更新", style = { flexGrow = 1 } };
			_updateButton.SetTintColor(Color.green);
			_updateButton.TrackPropertyValue(IsBusyProperty, property => _updateButton.SetEnabled(!property.boolValue));
			_actionButtonsContainer.Add(_updateButton);
			var removeButton = new Button(RemoveSheetContentInfo) { text = "移除", style = { flexGrow = 1 } };
			removeButton.SetTintColor(Color.red);
			_actionButtonsContainer.Add(removeButton);
			Add(_actionButtonsContainer);
		}

		protected virtual async void UpdateSheetContentInfo()
		{
			var sheetContentInfo = ItemProperty.GetValue<SheetContentInfo>();
			if (sheetContentInfo == null || GoogleSpreadsheetLoader == null)
				return;
			
			await GoogleSpreadsheetLoader.UpdateSheetContentInfo(sheetContentInfo);
		}

		protected virtual void RemoveSheetContentInfo() => Root.DeleteItem(Index);
		
		public override void Refresh(int index, SerializedProperty itemProperty, bool isAllowReordering, bool isAllowDisable, bool isAllowDuplicate, bool isAllowDelete, bool isAllowCopyPaste)
		{
			base.Refresh(index, itemProperty, isAllowReordering, isAllowDisable, isAllowDuplicate, isAllowDelete, isAllowCopyPaste);
			_sheetContentField.SetValueWithoutNotify(SheetContentProperty.GetValue<SheetContent>());
		}
	}
}