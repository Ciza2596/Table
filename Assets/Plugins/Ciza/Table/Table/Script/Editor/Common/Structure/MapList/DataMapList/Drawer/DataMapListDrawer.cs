using CizaTable.Editor.MapListVisual;
using UnityEditor;

namespace CizaTable.Editor
{
    [CustomPropertyDrawer(typeof(BDataMapList<>), true)]
    public class DataMapListDrawer : BMapListDrawer
    {
        protected virtual string KeyLabel => "Key";
        protected virtual string ValueLabel => "Data Values";

        protected override BMapListVE CreateListVE()
        {
            var listVE = new DataMapListVE(KeyLabel, ValueLabel, Property);
            listVE.Initialize();
            return listVE;
        }
    }
}
