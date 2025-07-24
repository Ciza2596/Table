using UnityEngine.Scripting;

namespace CizaTable.Editor
{
	public class ItemSortManipulator : BSortManipulator<ItemVE>
	{
		[Preserve]
		public ItemSortManipulator(IListVE list) : base(list, false, true) { }
	}
}