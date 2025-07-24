using System;
using CizaTable.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace GoogleSpreadsheetLoader.Editor
{
	public class SpreadsheetInfoVE : ItemVE
	{
		[field:NonSerialized]
		protected SheetInfosVE _sheetInfosVE;
		protected virtual string SpreadsheetIdPath => "_spreadsheetId";
		protected virtual string SheetContentPathPath => "_sheetContentPath";
		protected virtual string SpreadsheetNamePath => "_spreadsheetName";

		protected virtual string SheetInfosPath => "_sheetInfos";

		protected virtual SerializedProperty SpreadsheetIdProperty => ItemProperty.FindPropertyRelative(SpreadsheetIdPath);
		protected virtual SerializedProperty SheetContentPathProperty => ItemProperty.FindPropertyRelative(SheetContentPathPath);
		protected virtual SerializedProperty SpreadsheetNameProperty => ItemProperty.FindPropertyRelative(SpreadsheetNamePath);

		protected virtual SerializedProperty SheetInfosProperty => ItemProperty.FindPropertyRelative(SheetInfosPath);
		public override string Title => SpreadsheetName;
		

		[Preserve]
		public SpreadsheetInfoVE(ListVE root, SerializedProperty itemProperty) : base(root, itemProperty) { }

		public virtual string SpreadsheetName
		{
			get => SpreadsheetNameProperty.GetValue<string>();
			protected set => SpreadsheetNameProperty.SetValue(value);
		}

		protected override void CreateBodyContent()
		{
			_body.Add(CreatePropertyField(SpreadsheetIdProperty));

			_body.Add(CreatePropertyField(SheetContentPathProperty));
			_body.Add(new SmallSpaceVE());
			_body.Add(new Button(() => WebUtils.OpenGoogleSpreadSheetUrl(SpreadsheetIdProperty.stringValue)) { text = "Open Spreadsheet Web", style = { flexGrow = 1, marginLeft = 3, marginRight = -2 } });
			
			_body.Add(new SmallSpaceVE());
			_sheetInfosVE = new SheetInfosVE(SheetInfosProperty);
			_sheetInfosVE.Initialize();
			_body.Add(_sheetInfosVE);
		}

		protected virtual PropertyField CreatePropertyField(SerializedProperty property)
		{
			var field = new PropertyField(property);
			field.BindProperty(property);
			return field;
		}

		public override void Refresh(int index, SerializedProperty itemProperty, bool isAllowReordering, bool isAllowDisable, bool isAllowDuplicate, bool isAllowDelete, bool isAllowCopyPaste)
		{
			base.Refresh(index, itemProperty, isAllowReordering, isAllowDisable, isAllowDuplicate, isAllowDelete, isAllowCopyPaste);
			_sheetInfosVE.SetListProperty(SheetInfosProperty);
			_sheetInfosVE.Refresh();
		}
	}
}