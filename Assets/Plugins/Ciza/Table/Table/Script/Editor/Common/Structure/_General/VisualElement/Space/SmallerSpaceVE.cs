using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public class SmallerSpaceVE : VisualElement
	{
		[Preserve]
		public SmallerSpaceVE() =>
			style.height = new StyleLength(5);
	}
}