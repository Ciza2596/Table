using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GoogleSpreadsheetLoader.Editor
{
    public class GoogleSpreadsheetLoaderEditor : EditorWindow
    {
        //private variable
        private readonly string[] _toolbarTexts = { "GoogleSpreadsheetLoader", "Config" };
        private int _toolbarIndex = 0;

        private readonly string CONFIG_ASSET_PATH_KEY = $"GoogleSpreadsheetLoader.{nameof(ConfigAssetPath)}";
        
        private string ConfigAssetPath
        {
            get => PlayerPrefs.GetString(CONFIG_ASSET_PATH_KEY);
            set
            {
                PlayerPrefs.SetString(CONFIG_ASSET_PATH_KEY, value);
                PlayerPrefs.Save();
            }
        }


        private string CURRENT_CONFIG_ASSET_PATH_KEY = $"GoogleSpreadsheetLoader.{nameof(CurrentConfigAssetPath)}";

        private string CurrentConfigAssetPath
        {
            get => PlayerPrefs.GetString(CURRENT_CONFIG_ASSET_PATH_KEY);
            set
            {
                PlayerPrefs.SetString(CURRENT_CONFIG_ASSET_PATH_KEY, value);
                PlayerPrefs.Save();
            }
        }


        private Vector2 _scrollPosition;
        private UnityEditor.Editor _googleSpreadsheetLoaderEditor;


        //private method
        [MenuItem("Tools/CizaModule/GoogleSpreadsheetLoader")]
        private static void ShowWindow() => GetWindow<GoogleSpreadsheetLoaderEditor>("GoogleSpreadsheetLoader");
        

        private void OnGUI()
        {
            ToolBarArea();

            switch (_toolbarIndex)
            {
                case 0:
                    GoogleSpreadsheetLoaderArea();
                    break;
                case 1:
                    ConfigArea();
                    break;
            }
        }


        private void ToolBarArea()
        {
            GUILayout.BeginHorizontal();
            _toolbarIndex = GUILayout.Toolbar(_toolbarIndex, _toolbarTexts);
            GUILayout.EndHorizontal();
        }

        private void GoogleSpreadsheetLoaderArea()
        {
            if (CurrentConfigAssetPath != ConfigAssetPath || _googleSpreadsheetLoaderEditor is null)
            {
                CurrentConfigAssetPath = ConfigAssetPath;
                var googleSpreadsheetLoader = AssetDatabase.LoadAssetAtPath<Object>(CurrentConfigAssetPath);
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


        private void ConfigArea()
        {
            EditorGUILayout.Space();
            ConfigAssetPath = GetAssetPathAndOpenWindow("Config Path", ConfigAssetPath);
            EditorGUILayout.Space();
        }


        private string GetAssetPathAndOpenWindow(string label, string originPath)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var path = EditorGUILayout.TextField(label, originPath);
                if (GUILayout.Button("Select", EditorStyles.miniButton, GUILayout.Width(65)))
                {
                    path = EditorUtility.OpenFilePanel("Folder Path", originPath, "");
                    var dataPath = Application.dataPath;
                    dataPath = dataPath.Replace("Assets", "");
                    path = path.Replace(dataPath, "");
                }


                return string.IsNullOrWhiteSpace(path) ? originPath : path;
            }
        }
    }
}