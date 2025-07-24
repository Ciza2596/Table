using CizaTable.Editor;
using UnityEditor;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    public class SheetContentInfosVE : ListVE
    {
        public SheetContentInfosVE(SerializedProperty listProperty) : base(listProperty) { }
        
        protected override ItemVE CreateItem(SerializedProperty itemProperty)
        {
            var sheetContentInfoVE = new SheetContentInfoVE(this, itemProperty);
            sheetContentInfoVE.Initialize();
            return sheetContentInfoVE;
        }
    }
}
