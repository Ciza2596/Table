using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
    public class SheetContentInfosVE : ListVE
    {
        [Preserve]
        public SheetContentInfosVE(SerializedProperty listProperty) : base(listProperty) { }


        public override void DeleteItem(int index)
        {
            if(index < 0) return;
            
            ListProperty.serializedObject.Update();
            var sheetContent = ItemsProperty.GetArrayElementAtIndex(index).FindPropertyRelative("_sheetContent").GetValue<SheetContent>();
            var assetPath = AssetDatabase.GetAssetPath(sheetContent);
            
            ItemsProperty.DeleteArrayElementAtIndex(index);
            SerializationUtils.ApplyUnregisteredSerialization(ListProperty.serializedObject);
            Refresh();
            
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"[SubSheetContentInfo::Remove] Remove content file : {assetPath}.");
        }

        protected override void DerivedInitialize()
        {
            base.DerivedInitialize();
            this.SetMargin(3, -2, 0, 0);
            Refresh();
        }
		
        protected override void SetupHead()
        {
            _head.Add(CreateHeadLabel("Scriptable Object", 60));
            _head.Add(CreateHeadLabel("動作", 40, true));
            _head.style.marginBottom = 0;
            _head.style.backgroundColor = new Color(0.25f, 0.25f, 0.25f);
        }

        protected override void SetupFoot()
        {
            _foot.style.height = 0;
            _foot.SetMargin(0);
        }
        
        protected override ItemVE CreateItem(SerializedProperty itemProperty)
        {
            var sheetContentInfoVE = new SheetContentInfoVE(this, itemProperty);
            sheetContentInfoVE.Initialize();
            return sheetContentInfoVE;
        }
        
        protected virtual Label CreateHeadLabel(string text, float widthPercentage, bool isLast = false)
        {
            var label = new Label(text);
            label.style.flexGrow = 1;
            label.style.flexShrink = 1;
            label.style.overflow = Overflow.Hidden;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.width = Length.Percent(widthPercentage);
            label.SetBorder(1, Color.black, isLast ? SideKinds.All : SideKinds.NoRight);
            return label;
        }
    }
}
