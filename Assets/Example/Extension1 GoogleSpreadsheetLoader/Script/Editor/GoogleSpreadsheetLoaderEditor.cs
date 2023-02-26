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

        private readonly string _configGuidKey = $"GoogleSpreadsheetLoader.{nameof(ConfigGuid)}";

        private string ConfigGuid
        {
            get => PlayerPrefs.GetString(_configGuidKey);
            set
            {
                PlayerPrefs.SetString(_configGuidKey, value);
                PlayerPrefs.Save();
            }
        }


        private GoogleSpreadsheetLoader _config;

        private GoogleSpreadsheetLoader Config
        {
            get
            {
                if (_config is null)
                    _config = GetObject<GoogleSpreadsheetLoader>(ConfigGuid);

                return _config;
            }

            set
            {
                _config = value;
                var guid = _config is null
                    ? string.Empty
                    : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_config));
                ConfigGuid = guid;
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
            if(Config is null)
                return;
            
            if (_googleSpreadsheetLoaderEditor is null)
                _googleSpreadsheetLoaderEditor = UnityEditor.Editor.CreateEditor(Config);

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
            Config = EditorGUILayout.ObjectField("Config", Config, typeof(GoogleSpreadsheetLoader)) as GoogleSpreadsheetLoader;
            EditorGUILayout.Space();
        }
        

        private T GetObject<T>(string guid) where T : Object
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return obj;
        }
    }
}