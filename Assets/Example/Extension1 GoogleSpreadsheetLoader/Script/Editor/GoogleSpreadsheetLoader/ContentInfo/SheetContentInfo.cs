using System;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    [Serializable]
    public class SheetContentInfo
    {
        //private variable
        [TableColumnWidth(200)] [VerticalGroup("ScriptableObject")] [ReadOnly] [SerializeField]
        private SheetContent _sheetContent;

        private GoogleSpreadsheetLoader _googleSpreadsheetLoader;

        private string _sheetInfoId;
        
        private bool _isBusy;

        private string _spreadSheetId;
        private string _sheetId;


        //constructor
        public SheetContentInfo(string sheetInfoId, SheetContent sheetContent,
            GoogleSpreadsheetLoader googleSpreadsheetLoader)
        {
            _sheetInfoId = sheetInfoId;
            _sheetContent = sheetContent;
            _googleSpreadsheetLoader = googleSpreadsheetLoader;
        }


        //public variable
        public string SheetInfoId => _sheetInfoId;
        
        
        public bool IsBusy => _isBusy;

        public string SpreadSheetId => _spreadSheetId;
        public string SheetId => _sheetId;


        //public method
        public void SetIsBusy(bool isBusy) =>
            _isBusy = isBusy;


        //private method
        [HorizontalGroup("動作")]
        [Button("更新")]
        [GUIColor(0, 1, 0)]
        [DisableIf("IsBusy")]
        public async Task Update()
        {
            try
            {
                await _googleSpreadsheetLoader.UpdateSheetContent(this);
            }
            catch
            {
                _isBusy = false;
            }
        }
        
        public void Update(string sheetName, string csv) =>
            _sheetContent.Update(sheetName, csv);

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
    }
}