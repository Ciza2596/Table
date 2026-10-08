using UnityEngine.Scripting;

namespace CizaTable.Editor
{
	public class ItemSortManipulator : BSortManipulator<ItemVE>
	{
		// CONSTRUCTOR: --------------------------------------------------------------------- 
		
		[Preserve]
		public ItemSortManipulator(IListVE list) : base(list, false, true) { }
	}
}