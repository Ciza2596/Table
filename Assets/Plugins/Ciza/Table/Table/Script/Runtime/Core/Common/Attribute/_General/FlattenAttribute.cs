using System;

namespace CizaTable
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
	public class FlattenAttribute : Attribute { }
}