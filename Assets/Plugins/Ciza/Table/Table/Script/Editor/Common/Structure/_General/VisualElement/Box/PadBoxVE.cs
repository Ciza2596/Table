using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public class PadBoxVE : VisualElement
	{
		protected virtual string[] USSPaths => new[] { "PadBox" };

		[Preserve]
		public PadBoxVE()
		{
			foreach (var sheet in StyleSheetUtils.GetStyleSheets(USSPaths))
				styleSheets.Add(sheet);
		}
	}
}