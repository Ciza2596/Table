using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	public class OverrideDrawerAttribute : PropertyAttribute
	{
		public readonly string Label;
		
		[Preserve]
		public OverrideDrawerAttribute() : base(true) => Label = null;

		[Preserve]
		public OverrideDrawerAttribute(string label) : base(true) => Label = label;
	}
}