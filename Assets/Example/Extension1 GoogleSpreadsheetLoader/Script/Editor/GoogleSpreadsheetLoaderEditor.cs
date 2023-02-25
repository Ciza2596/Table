using UnityEditor;
using UnityEngine;

namespace GoogleSpreadsheetLoader.Editor
{
    public class GoogleSpreadsheetLoaderEditor : EditorWindow
    {
        //private variable
        private readonly string[] _toolbarTexts = { "GoogleSpreadsheetLoader", "Config" };
        private int _toolbarIndex = 0;

        private string _assetPath;
        private string _currentAssetPath;

        private Vector2 _scrollPosition;
        private UnityEditor.Editor _googleSpreadsheetLoaderEditor;


        //private method
        [MenuItem("Tools/CizaModule/GoogleSpreadsheetLoader")]
        private static void ShowWindow() => GetWindow<GoogleSpreadsheetLoaderEditor>("GoogleSpreadsheetLoaderEditor");

        private void OnGUI()
        {
            ToolBarArea();

            switch (_toolbarIndex)
            {
                case 0:
                    GoogleSpreadsheetLoader();
                    break;
                case 1:
                    Config();
                    break;
            }
        }


        private void ToolBarArea()
        {
            GUILayout.BeginHorizontal();
            _toolbarIndex = GUILayout.Toolbar(_toolbarIndex, _toolbarTexts);
            GUILayout.EndHorizontal();
        }

        private void GoogleSpreadsheetLoader()
        {
            if (_currentAssetPath != _assetPath)
            {
                _currentAssetPath = _assetPath;
                var googleSpreadsheetLoader = AssetDatabase.LoadAssetAtPath<Object>(_currentAssetPath);
                _googleSpreadsheetLoaderEditor = UnityEditor.Editor.CreateEditor(googleSpreadsheetLoader);
            }

            if (_googleSpreadsheetLoaderEditor is null)
                return;

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.Space(15);
            
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            _googleSpreadsheetLoaderEditor.OnInspectorGUI();
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.Space(15);
            
            EditorGUILayout.EndHorizontal();
        }


        private void Config()
        {
            EditorGUILayout.Space();
            _assetPath = GetAssetPathAndOpenWindow("Config Path", _assetPath);
            EditorGUILayout.Space();
        }


        private string GetAssetPathAndOpenWindow(string label, string originPath)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var path = EditorGUILayout.TextField(label, originPath);
                if (GUILayout.Button("Select", EditorStyles.miniButton, GUILayout.Width(65)))
                {
                    path = EditorUtility.OpenFilePanel("Folder Path", "Assets", "");
                    var dataPath = Application.dataPath;
                    dataPath = dataPath.Replace("Assets", "");
                    path = path.Replace(dataPath, "");
                }


                return string.IsNullOrWhiteSpace(path) ? originPath : path;
            }
        }
    }
}