using UnityEngine;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public static class VisualElementUtils
	{
		public static void SetIsVisible(this VisualElement visualElement, bool isVisible) => visualElement.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;

		public static void SetPadding(this VisualElement element, float left, float right, float top, float bottom)
		{
			element.style.paddingLeft = left;
			element.style.paddingRight = right;
			element.style.paddingTop = top;
			element.style.paddingBottom = bottom;
		}

		public static void SetBorder(this VisualElement element, float thickness, Color color = default, SideKinds sideKinds = SideKinds.All)
		{
			if (sideKinds.HasFlag(SideKinds.Left))
			{
				element.style.borderLeftWidth = thickness;
				element.style.borderLeftColor = color;
			}

			if (sideKinds.HasFlag(SideKinds.Right))
			{
				element.style.borderRightWidth = thickness;
				element.style.borderRightColor = color;
			}

			if (sideKinds.HasFlag(SideKinds.Top))
			{
				element.style.borderTopWidth = thickness;
				element.style.borderTopColor = color;
			}

			if (sideKinds.HasFlag(SideKinds.Bottom))
			{
				element.style.borderBottomWidth = thickness;
				element.style.borderBottomColor = color;
			}
		}

		public static void SetBorder(this VisualElement element, Color color, SideKinds sideKinds = SideKinds.All)
		{
			if (sideKinds.HasFlag(SideKinds.Left))
				element.style.borderLeftColor = color;

			if (sideKinds.HasFlag(SideKinds.Right))
				element.style.borderRightColor = color;

			if (sideKinds.HasFlag(SideKinds.Top))
				element.style.borderTopColor = color;

			if (sideKinds.HasFlag(SideKinds.Bottom))
				element.style.borderBottomColor = color;
		}

		public static void SetMargin(this VisualElement element, float margin, SideKinds sideKinds = SideKinds.All)
		{
			if (sideKinds.HasFlag(SideKinds.Left))
				element.style.marginLeft = margin;
			if (sideKinds.HasFlag(SideKinds.Right))
				element.style.marginRight = margin;
			if (sideKinds.HasFlag(SideKinds.Top))
				element.style.marginTop = margin;
			if (sideKinds.HasFlag(SideKinds.Bottom))
				element.style.marginBottom = margin;
		}

		public static void SetMargin(this VisualElement element, float left, float right, float top, float bottom)
		{
			element.style.marginLeft = left;
			element.style.marginRight = right;
			element.style.marginTop = top;
			element.style.marginBottom = bottom;
		}

		public static void SetColor(this VisualElement element, ColorSet colorSet)
		{
			if (element == null || colorSet == null)
				return;

			element.style.backgroundColor = colorSet.BackgroundColor;
			element.style.color = colorSet.TextColor;
			element.SetBorder(colorSet.BorderColor);
		}

		public static void SetTintColor(this VisualElement element, Color tint, bool isApplyToChildren = true)
		{
			if(tint == Color.white)
				return;
			if(element.style.backgroundImage != null)
				element.style.unityBackgroundImageTintColor = tint;
			if (element is Image image)
				image.tintColor = tint;
			else if (element.userData is ColorSet colorSet)
			{
				colorSet.TintMultiplier = tint;
				element.userData = colorSet;
				element.SetColor(element.enabledSelf ? colorSet.TintSet : colorSet.DisabledSet);
			}
			else
			{
				element.RegisterCallbackOnce<GeometryChangedEvent>(_ =>
				{
					colorSet = new ColorSet(element.resolvedStyle.backgroundColor, element.resolvedStyle.borderLeftColor, element.resolvedStyle.color) { TintMultiplier = tint };
					element.userData = colorSet;
					element.SetTintColor(tint);

					element.RegisterCallback<GeometryChangedEvent>(_ =>
					{
						if (element.userData is ColorSet set)
							element.SetColor(element.enabledSelf ? set.TintSet : set.DisabledSet);
					});
					element.RegisterCallback<PointerEnterEvent>(_ => element.SetColor((element.userData as ColorSet)?.HoverSet));
					element.RegisterCallback<PointerLeaveEvent>(_ => element.SetColor((element.userData as ColorSet)?.TintSet));
					element.RegisterCallback<PointerDownEvent>(_ => element.SetColor((element.userData as ColorSet)?.ActiveSet), TrickleDown.TrickleDown);
					element.RegisterCallback<PointerUpEvent>(_ => element.SetColor((element.userData as ColorSet)?.HoverSet), TrickleDown.TrickleDown);
				});
			}

			if (!isApplyToChildren)
				return;

			foreach (var child in element.Children())
				child.SetTintColor(tint);
		}
	}
}