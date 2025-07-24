using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GoogleSpreadsheetLoader.Editor
{
	//[CustomPropertyDrawer(typeof(List<SheetInfo>))]
	public class SheetInfosDrawer : PropertyDrawer
	{

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var sheetInfosVE = new SheetInfosVE(property);
			sheetInfosVE.Initialize();
			return sheetInfosVE;
		}
	}
}