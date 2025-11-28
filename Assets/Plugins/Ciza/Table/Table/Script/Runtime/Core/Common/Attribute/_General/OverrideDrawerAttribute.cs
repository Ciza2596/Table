using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	public class OverrideDrawerAttribute : PropertyAttribute
	{
		[Preserve]
		public OverrideDrawerAttribute() : base(true) { }
	}
}