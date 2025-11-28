using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	[CustomEditor(typeof(GoogleSpreadsheetLoader))]
	public class GoogleSpreadsheetLoaderEditor : UnityEditor.Editor
	{
		[field: NonSerialized]
		protected SpreadsheetInfosVE _spreadsheetInfosVE;

		[field: NonSerialized]
		protected SpreadsheetContentInfosVE _usedSpreadsheetContentInfosVE;
		
		[field: NonSerialized]
		protected Button _updateSpreadsheetPreviewButton;
		
		[field: NonSerialized]
		protected Button _updateUsedSpreadsheetContentButton;

		protected virtual string WebAppUrlPath => "_webAppUrl";
		protected virtual string SpreadsheetInfosPath => "_spreadsheetInfos";
		protected virtual string UsedSpreadsheetContentInfosPath => "_usedSpreadsheetContentInfos";
		
		protected virtual string IsBusyPath => "_isBusy";

		protected virtual SerializedProperty WebAppUrlProperty => serializedObject.FindProperty(WebAppUrlPath);
		protected virtual SerializedProperty SpreadsheetInfosProperty => serializedObject.FindProperty(SpreadsheetInfosPath);
		protected virtual SerializedProperty UsedSpreadsheetContentInfosProperty => serializedObject.FindProperty(UsedSpreadsheetContentInfosPath);
		protected virtual SerializedProperty IsBusyProperty => serializedObject.FindProperty(IsBusyPath);
		
		protected virtual GoogleSpreadsheetLoader GoogleSpreadsheetLoader => target as GoogleSpreadsheetLoader;

		public override VisualElement CreateInspectorGUI()
		{
			var root = new VisualElement();
			root.Add(new Label("Google Web Service") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 5 } });
			root.Add(new VisualElement() { style = { height = 1, flexGrow = 1, marginBottom = 5, backgroundColor = new Color(0.35f, 0.35f, 0.35f) } });
			root.Add(new Button(WebUtils.OpenGoogleScriptPage) { text = "Open Google App Script Web", style = { marginLeft = 0, marginRight = 0, flexGrow = 1 } });
			root.Add(new PropertyField(WebAppUrlProperty));
			root.TrackPropertyValue(IsBusyProperty, property =>
			{
				var isBusy = property.boolValue;
				_updateSpreadsheetPreviewButton.SetEnabled(!isBusy);
				_updateUsedSpreadsheetContentButton.SetEnabled(!isBusy);
			});

			root.Add(new SmallerSpaceVE());
			root.Add(new Label("Spreadsheet Preview") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 5 } });
			root.Add(new VisualElement() { style = { height = 1, flexGrow = 1, marginBottom = 5, backgroundColor = new Color(0.35f, 0.35f, 0.35f) } });

			var spreadsheetInfosBox = new BoxVE(SpreadsheetInfosProperty);
			_spreadsheetInfosVE = new SpreadsheetInfosVE(SpreadsheetInfosProperty, false);
			_spreadsheetInfosVE.Initialize();
			spreadsheetInfosBox.Initialize(SpreadsheetInfosProperty.displayName, _spreadsheetInfosVE);
			root.Add(spreadsheetInfosBox);
			_updateSpreadsheetPreviewButton = CreateButton("Update Spreadsheet Preview", UpdateSpreadsheetPreview, Color.green);
			root.Add(_updateSpreadsheetPreviewButton);

			root.Add(new SmallerSpaceVE());
			root.Add(new Label("Used Sheet Content") { style = { unityFontStyleAndWeight = FontStyle.Bold, marginTop = 5 } });
			root.Add(new VisualElement() { style = { height = 1, flexGrow = 1, marginBottom = 5, backgroundColor = new Color(0.35f, 0.35f, 0.35f) } });

			var usedSpreadsheetContentInfosBox = new BoxVE(UsedSpreadsheetContentInfosProperty);
			_usedSpreadsheetContentInfosVE = new SpreadsheetContentInfosVE(UsedSpreadsheetContentInfosProperty, false);
			_usedSpreadsheetContentInfosVE.Initialize();
			usedSpreadsheetContentInfosBox.Initialize(UsedSpreadsheetContentInfosProperty.displayName, _usedSpreadsheetContentInfosVE);
			root.Add(usedSpreadsheetContentInfosBox);

			var buttonContainer = new VisualElement() { style = { flexDirection = FlexDirection.Row } };
			_updateUsedSpreadsheetContentButton = CreateButton("Update All Used Sheet Contents", UpdateUsedSpreadsheetContentInfos, Color.green);
			buttonContainer.Add(_updateUsedSpreadsheetContentButton);
			buttonContainer.Add(CreateButton("Remove All Used Sheet Contents", RemoveUsedSpreadsheetContentInfos, Color.red));
			root.Add(buttonContainer);

			var busyButton = CreateButton("Reset Busy", ResetBusy, Color.cyan);
			root.Add(busyButton);
			return root;
		}

		protected virtual Button CreateButton(string text, Action onClick, Color color)
		{
			var button = new Button(onClick) { text = text, style = { flexGrow = 1 } };
			button.SetTintColor(color);
			return button;
		}

		protected virtual async void UpdateSpreadsheetPreview()
		{
			if (GoogleSpreadsheetLoader == null)
				return;

			await GoogleSpreadsheetLoader.UpdateSpreadsheetPreview();
			_spreadsheetInfosVE.Refresh();
		}
		
		protected virtual async void UpdateUsedSpreadsheetContentInfos()
		{
			if (GoogleSpreadsheetLoader == null)
				return;

			await GoogleSpreadsheetLoader.UpdateAllUsedSheetContentInfos();
			_usedSpreadsheetContentInfosVE.Refresh();
		}

		protected virtual void RemoveUsedSpreadsheetContentInfos()
		{
			GoogleSpreadsheetLoader?.RemoveAllUsedSheetContentInfos();
			_usedSpreadsheetContentInfosVE.SetListProperty(UsedSpreadsheetContentInfosProperty);
			_usedSpreadsheetContentInfosVE.Refresh();
		}
		
		protected virtual void ResetBusy() => GoogleSpreadsheetLoader?.ResetBusy();
		
		
	}
}