using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	public abstract class BTagAttribute : PropertyAttribute
	{
		[Preserve]
		protected BTagAttribute(bool applyToCollection) : base(applyToCollection) { }
	}
}