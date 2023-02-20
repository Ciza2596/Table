using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoogleSheetLoader.Editor
{
    [Serializable]
    public class SubSheetContentInfo
    {
        //private variable
        [TableColumnWidth(200)] [VerticalGroup("ScriptableObject")] [ReadOnly]
        private SubSheetContent _subSheetContent;

        private GoogleSheetLoader _googleSheetLoader;

        private bool _isBusy;
        
        private string _sheetId;
        private string _subSheetId;


        //constructor
        public SubSheetContentInfo(SubSheetContent subSheetContent,
            GoogleSheetLoader googleSheetLoader)
        {
            _subSheetContent = subSheetContent;
            _googleSheetLoader = googleSheetLoader;
        }


        //public variable
        public bool IsBusy => _isBusy;

        public string SheetId => _sheetId;
        public string SubSheetId => _subSheetId;


        public SubSheetContent SubSheetContent => _subSheetContent;

        
        //public method
        public void SetIsBusy(bool isBusy) =>
            _isBusy = isBusy;
        

        //private method
        [HorizontalGroup("動作")]
        [Button("更新")]
        [GUIColor(0, 1, 0)]
        [DisableIf("IsBusy")]
        public void Update()
        {
            try
            {
                _googleSheetLoader.UpdateSubSheetContent(this);
            }
            catch (Exception ex)
            {
                _isBusy = false;
            }
        }

        [HorizontalGroup("動作")]
        [GUIColor(1, 0, 0)]
        [Button("移除")]
        [DisableIf("IsBusy")]
        public void Remove()
        {
            var subSheetContent = _subSheetContent;
            _subSheetContent = null;

            var assetPath = AssetDatabase.GetAssetPath(subSheetContent);
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"[SubSheetContentInfo::Remove] Remove content file : {assetPath}.");
        }

        public void Initialize(string csv) =>
            _subSheetContent.Initialize(csv);
        
    }
}