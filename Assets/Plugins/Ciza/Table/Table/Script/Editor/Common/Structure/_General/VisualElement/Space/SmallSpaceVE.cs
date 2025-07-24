using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public class SmallSpaceVE : VisualElement
	{
		[Preserve]
		public SmallSpaceVE() =>
			style.height = new StyleLength(10);
	}
}