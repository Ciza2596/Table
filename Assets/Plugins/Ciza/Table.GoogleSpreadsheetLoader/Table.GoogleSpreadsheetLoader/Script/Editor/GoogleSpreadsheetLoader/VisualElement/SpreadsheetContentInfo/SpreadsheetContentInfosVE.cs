using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable.Editor
{
    public class SpreadsheetContentInfosVE : ListVE
    {
        [Preserve]
        public SpreadsheetContentInfosVE(SerializedProperty listProperty) : base(listProperty) { }

        protected override ItemVE CreateItem(SerializedProperty itemProperty)
        {
            var spreadsheetContentVE = new SpreadsheetContentInfoVE(this, itemProperty);
            spreadsheetContentVE.Initialize();
            return spreadsheetContentVE;
        }
    }
}
