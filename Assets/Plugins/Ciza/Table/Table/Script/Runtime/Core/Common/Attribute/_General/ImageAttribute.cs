using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class ImageAttribute : Attribute
	{
		private readonly IIcon m_Icon;

		// PROPERTIES: ----------------------------------------------------------------------------

		public Texture2D Image => this.m_Icon.Texture;

		// CONSTRUCTORS: --------------------------------------------------------------------------

		[Preserve]
		public ImageAttribute(Type iconType, ColorTheme.Type color) : this(iconType, ColorTheme.Get(color)) { }

		[Preserve]
		public ImageAttribute(Type iconType, Color color) : this(iconType, color, null) { }

		[Preserve]
		public ImageAttribute(Type iconType, ColorTheme.Type iconColor, Type overlayType) : this(iconType, ColorTheme.Get(iconColor), overlayType) { }

		[Preserve]
		public ImageAttribute(Type iconType, Color iconColor, Type overlayType)
		{
			var overlay = overlayType != null ? Activator.CreateInstance(overlayType, Color.white, null) as IIcon : null;
			m_Icon = Activator.CreateInstance(iconType, iconColor, overlay) as IIcon;
		}
	}
}