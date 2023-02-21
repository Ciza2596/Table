using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class SheetContentInfo
    {
        //private variable
        [TableColumnWidth(200)] [VerticalGroup("ScriptableObject")] [ReadOnly]
        private SheetContent _sheetContent;

        private GoogleSpreadsheetLoader _googleSpreadsheetLoader;

        private bool _isBusy;
        
        private string _sheetId;
        private string _subSheetId;


        //constructor
        public SheetContentInfo(SheetContent sheetContent,
            GoogleSpreadsheetLoader googleSpreadsheetLoader)
        {
            _sheetContent = sheetContent;
            _googleSpreadsheetLoader = googleSpreadsheetLoader;
        }


        //public variable
        public bool IsBusy => _isBusy;

        public string SheetId => _sheetId;
        public string SubSheetId => _subSheetId;


        public SheetContent SheetContent => _sheetContent;

        
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
                _googleSpreadsheetLoader.UpdateSubSheetContent(this);
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
            var subSheetContent = _sheetContent;
            _sheetContent = null;

            var assetPath = AssetDatabase.GetAssetPath(subSheetContent);
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"[SubSheetContentInfo::Remove] Remove content file : {assetPath}.");
        }

        public void Initialize(string csv) =>
            _sheetContent.Initialize(csv);
        
    }
}