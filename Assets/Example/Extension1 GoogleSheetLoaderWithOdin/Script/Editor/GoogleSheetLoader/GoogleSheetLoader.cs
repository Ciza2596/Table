using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoogleSheetLoader.Editor
{
    [CreateAssetMenu(fileName = "GoogleSheetLoader", menuName = "GoogleSheetLoader")]
    public class GoogleSheetLoader : ScriptableObject
    {
        //private variable
        [Title("Google服務設定", "執行Google apps script位置設定")]
        private string[] _webServices;

        [SerializeField] private int _currentWebServiceIndex;

        [Title("靜態表設定", "設定需要從雲端下載的表單")] [VerticalGroup("SpreadsheetsSetting")]
        private SheetInfo[] _sheetInfos;

        [TableList(IsReadOnly = true)] private List<SheetContentInfo> _sheetContentInfos;


        //public method
        public void UpdateSubSheetContent(SubSheetContentInfo subSheetContentInfo)
        {
        }

        public void RemoveSubSheetContent(SubSheetContent subSheetContent)
        {
            var assetPath = AssetDatabase.GetAssetPath(subSheetContent);
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"[ContentManager:RemoveSubSheetContent] Remove content file : {assetPath}.");
        }
    }
}