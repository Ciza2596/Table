using System;
using UnityEngine.Scripting;

namespace CizaTable
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class DescriptionAttribute : Attribute, ISearchable
	{
		public string Description { get; }

		[Preserve]
		public DescriptionAttribute(string description)
		{
			Description = description.Trim();
		}

		public string Text => Description;
		public int Priority => 2;
	}
}