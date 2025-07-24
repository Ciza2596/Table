using System;
using UnityEngine.Scripting;

namespace CizaTable
{
	[Serializable]
	public class AddressMapList : MapList<string>
	{
		[Preserve]
		public AddressMapList() { }
	}
}