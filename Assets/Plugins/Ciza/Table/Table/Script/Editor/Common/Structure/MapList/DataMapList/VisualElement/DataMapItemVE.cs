using CizaTable.Editor.MapListVisual;
using UnityEditor;
using UnityEngine.Scripting;

namespace CizaTable.Editor
{
    public class DataMapItemVE : MapItemVE
    {
        protected override string ValuePath => "_dataValues";

        [Preserve]
        public DataMapItemVE(string keyLabel, string valueLabel, BMapListVE root, SerializedProperty itemProperty) : 
            base(keyLabel, valueLabel, root, itemProperty) { }
    }
}
