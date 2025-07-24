using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public class TitleLabelVE : Label
	{
		[Preserve]
		public TitleLabelVE()
		{
			style.unityFontStyleAndWeight = FontStyle.Bold;
			style.marginTop = new StyleLength(3);
			style.marginBottom = new StyleLength(3);
		}

		[Preserve]
		public TitleLabelVE(string text) : this() =>
			this.text = text;
	}
}