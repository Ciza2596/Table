using System;
using Sirenix.OdinInspector;

namespace GoogleSheetLoader.Editor
{
    [Serializable]
    public class SubSheetContentInfo
    {
        //private variable
        [TableColumnWidth(200)] [VerticalGroup("ScriptableObject")] [ReadOnly]
        private SubSheetContent _subSheetContent;

        private SheetContentInfo _sheetContentInfo;
        private GoogleSheetLoader _googleSheetLoader;

        private bool _isBusy;


        //constructor
        public SubSheetContentInfo(SubSheetContent subSheetContent, SheetContentInfo sheetContentInfo,
            GoogleSheetLoader googleSheetLoader)
        {
            _subSheetContent = subSheetContent;
            _sheetContentInfo = sheetContentInfo;
            _googleSheetLoader = googleSheetLoader;
        }


        //public variable
        public bool IsBusy => _isBusy;
        public SubSheetContent SubSheetContent => _subSheetContent;


        //private method
        [HorizontalGroup("動作")]
        [Button("更新")]
        [GUIColor(0, 1, 0)]
        [DisableIf("IsBusy")]
        private void Update()
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
        private void Remove()
        {
            _sheetContentInfo.RemoveSubSheetContentInfo(this);

            var subSheetContent = _subSheetContent;
            _subSheetContent = null;
            _googleSheetLoader.RemoveSubSheetContent(subSheetContent);
        }
    }
}