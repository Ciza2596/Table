using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoogleSheetLoader.Editor
{
    [CreateAssetMenu(fileName = "GoogleSheetLoader", menuName = "GoogleSheetLoader")]
    public class GoogleSheetLoader : ScriptableObject
    {
        [Title("Google服務設定", "執行Google apps script位置設定")]
        private string[] _webServices;

        [SerializeField]
        private int _currentWebServiceIndex;
        
        
        

    }

    [Serializable]
    public class PageId
    {
        public string ParentSheet;
    }

    [Serializable]
    public class InfoId
    {
        
    }
}
