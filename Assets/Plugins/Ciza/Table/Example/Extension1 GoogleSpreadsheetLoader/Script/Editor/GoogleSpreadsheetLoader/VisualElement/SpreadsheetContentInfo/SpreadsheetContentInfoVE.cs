using System;
using CizaTable.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace GoogleSpreadsheetLoader.Editor
{
    public class SpreadsheetContentInfoVE : ItemVE
    {
        [field:NonSerialized]
        protected SheetContentInfosVE _sheetContentInfosVE;

        protected virtual string SheetContentPathPath => "_sheetContentPath";
        protected virtual string SheetContentInfosPath => "_sheetContentInfos";
        protected virtual string SpreadsheetInfoIdPath => "_spreadsheetInfoId";
        
        protected virtual SerializedProperty SheetContentPathProperty => ItemProperty.FindPropertyRelative(SheetContentPathPath);
        protected virtual SerializedProperty SheetContentInfosProperty => ItemProperty.FindPropertyRelative(SheetContentInfosPath);
        protected virtual SerializedProperty SpreadsheetInfoIdProperty => ItemProperty.FindPropertyRelative(SpreadsheetInfoIdPath);

        public override string Title => SpreadsheetInfoIdProperty?.stringValue;

        [Preserve]
        public SpreadsheetContentInfoVE(SpreadsheetContentInfosVE root, SerializedProperty itemProperty) : base(root, itemProperty) { }

        protected override void CreateBodyContent()
        {
            var pathField = new TextField("Sheet Content存放路徑(建立物件時使用)");
            pathField.AddToClassList(AlignLabel.UNITY_ALIGN_FIELD_CLASS);
            pathField.BindProperty(SheetContentPathProperty);
            pathField.SetEnabled(false);
            _body.Add(pathField);
            
            _sheetContentInfosVE = new SheetContentInfosVE(SheetContentInfosProperty);
            _sheetContentInfosVE.Initialize();
            _body.Add(_sheetContentInfosVE);
        }

        public override void Refresh(int index, SerializedProperty itemProperty, bool isAllowReordering, bool isAllowDisable, bool isAllowDuplicate, bool isAllowDelete, bool isAllowCopyPaste)
        {
            base.Refresh(index, itemProperty, isAllowReordering, isAllowDisable, isAllowDuplicate, isAllowDelete, isAllowCopyPaste);
            _sheetContentInfosVE.SetListProperty(SheetContentInfosProperty);
            _sheetContentInfosVE.Refresh();
        }
    }
}
