using System;
using System.Threading.Tasks;
using CizaTable.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GoogleSpreadsheetLoader.Editor
{
	[CustomEditor(typeof(GoogleSpreadsheetLoader))]
	public class GoogleSpreadsheetLoaderEditor : UnityEditor.Editor
	{
		[field: NonSerialized]
		protected SpreadsheetInfosVE _spreadsheetInfosVE;

		[field: NonSerialized]
		protected SpreadsheetContentInfosVE _usedSpreadsheetContentInfosVE;

		protected virtual string WebAppUrlPath => "_webAppUrl";
		protected virtual string SpreadsheetInfosPath => "_spreadsheetInfos";
		protected virtual string UsedSpreadsheetContentInfosPath => "_usedSpreadsheetContentInfos";

		protected virtual SerializedProperty WebAppUrlProperty => serializedObject.FindProperty(WebAppUrlPath);
		protected virtual SerializedProperty SpreadsheetInfosProperty => serializedObject.FindProperty(SpreadsheetInfosPath);
		protected virtual SerializedProperty UsedSpreadsheetContentInfosProperty => serializedObject.FindProperty(UsedSpreadsheetContentInfosPath);

		public override VisualElement CreateInspectorGUI()
		{
			var root = new VisualElement();
			root.Add(new Label("Google Web Service") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 5 } });
			root.Add(new VisualElement() { style = { height = 1, flexGrow = 1, marginBottom = 5, backgroundColor = new Color(0.35f, 0.35f, 0.35f) } });
			root.Add(new Button(WebUtils.OpenGoogleScriptPage) { text = "Open Google App Script Web", style = { marginLeft = 0, marginRight = 0, flexGrow = 1 } });
			root.Add(new PropertyField(WebAppUrlProperty));

			root.Add(new SmallerSpaceVE());
			root.Add(new Label("Spreadsheet Preview") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 5 } });
			root.Add(new VisualElement() { style = { height = 1, flexGrow = 1, marginBottom = 5, backgroundColor = new Color(0.35f, 0.35f, 0.35f) } });

			var spreadsheetInfosBox = new BoxVE(SpreadsheetInfosProperty);
			_spreadsheetInfosVE = new SpreadsheetInfosVE(SpreadsheetInfosProperty);
			_spreadsheetInfosVE.Initialize();
			spreadsheetInfosBox.Initialize(SpreadsheetInfosProperty.displayName, _spreadsheetInfosVE);
			root.Add(spreadsheetInfosBox);
			root.Add(new Button(UpdateSpreadsheetPreview) { text = "Update Spreadsheet Preview" });

			root.Add(new SmallerSpaceVE());
			root.Add(new Label("Used Sheet Content") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 5 } });
			root.Add(new VisualElement() { style = { height = 1, flexGrow = 1, marginBottom = 5, backgroundColor = new Color(0.35f, 0.35f, 0.35f) } });

			var usedSpreadsheetContentInfosBox = new BoxVE(UsedSpreadsheetContentInfosProperty);
			_usedSpreadsheetContentInfosVE = new SpreadsheetContentInfosVE(UsedSpreadsheetContentInfosProperty);
			_usedSpreadsheetContentInfosVE.Initialize();
			usedSpreadsheetContentInfosBox.Initialize(UsedSpreadsheetContentInfosProperty.displayName, _usedSpreadsheetContentInfosVE);
			root.Add(usedSpreadsheetContentInfosBox);

			var buttonContainer = new VisualElement() { style = { flexDirection = FlexDirection.Row } };
			buttonContainer.Add(CreateButton("Update All Used Sheet Contents", Color.green));
			buttonContainer.Add(CreateButton("Remove All Used Sheet Contents", Color.red));
			root.Add(buttonContainer);

			var busyButton = CreateButton("Reset Busy", Color.cyan);
			root.Add(busyButton);
			return root;
		}

		protected virtual Button CreateButton(string text, Color color)
		{
			var button = new Button() { text = text, style = { flexGrow = 1 } };

			button.SetTintColor(color);
			return button;
		}

		protected virtual async void UpdateSpreadsheetPreview()
		{
			if (serializedObject.targetObject is not GoogleSpreadsheetLoader spreadsheetLoader)
				return;

			await spreadsheetLoader.UpdateSpreadsheets();
			_spreadsheetInfosVE.Refresh();
		}
	}
}