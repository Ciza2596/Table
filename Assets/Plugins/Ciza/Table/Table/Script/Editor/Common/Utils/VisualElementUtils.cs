using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace CizaTable.Editor
{
	public static class VisualElementUtils
	{
		public static PopupField<T> CreatePopupField<T>(List<T> options, SerializedProperty property)
		{
			T defaultValue = property.GetValue<string>().CheckHasValue() && options.Contains(property.GetValue<T>()) ? property.GetValue<T>() : options[0];
			var popupField = new PopupField<T>(options, defaultValue) { label = property.displayName };
			property.SetValue(defaultValue);
			popupField.AddToClassList(AlignLabel.UNITY_ALIGN_FIELD_CLASS);

			popupField.RegisterValueChangedCallback(evt => { property.SetValue(evt.newValue); });
			return popupField;
		}

		public static bool CheckIsVisible(this VisualElement visualElement) => visualElement.style.display == DisplayStyle.Flex;

		public static void SetIsVisible(this VisualElement visualElement, bool isVisible) => visualElement.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;

		public static T QFromParent<T>(this VisualElement visualElement) where T : VisualElement
		{
			var currentVE = visualElement.parent;
			while (currentVE != null)
			{
				if (currentVE is T match)
					return match;

				currentVE = currentVE.parent;
			}

			return null;
		}

		public static void SetPadding(this VisualElement element, float padding, SideKinds sideKinds = SideKinds.All)
		{
			if (sideKinds.HasFlag(SideKinds.Left))
				element.style.paddingLeft = padding;
			if (sideKinds.HasFlag(SideKinds.Right))
				element.style.paddingRight = padding;
			if (sideKinds.HasFlag(SideKinds.Top))
				element.style.paddingTop = padding;
			if (sideKinds.HasFlag(SideKinds.Bottom))
				element.style.paddingBottom = padding;
		}

		public static void SetPadding(this VisualElement element, float left, float right, float top, float bottom)
		{
			element.style.paddingLeft = left;
			element.style.paddingRight = right;
			element.style.paddingTop = top;
			element.style.paddingBottom = bottom;
		}

		public static void SetHorizontalPadding(this VisualElement element, float padding) => SetPadding(element, padding, SideKinds.Horizontal);

		public static void SetVerticalPadding(this VisualElement element, float padding) => SetPadding(element, padding, SideKinds.Vertical);

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

		public static void SetHorizontalBorder(this VisualElement element, float thickness, Color color = default) => SetBorder(element, thickness, color, SideKinds.Horizontal);

		public static void SetVerticalBorder(this VisualElement element, float thickness, Color color = default) => SetBorder(element, thickness, color, SideKinds.Vertical);

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

		public static void SetHorizontalMargin(this VisualElement element, float margin) => SetMargin(element, margin, SideKinds.Horizontal);

		public static void SetVerticalMargin(this VisualElement element, float margin) => SetMargin(element, margin, SideKinds.Vertical);

		public static void SetCorner(this VisualElement element, float radius, CornerKinds cornerKinds = CornerKinds.All)
		{
			if (cornerKinds.HasFlag(CornerKinds.TopLeft))
				element.style.borderTopLeftRadius = radius;
			if (cornerKinds.HasFlag(CornerKinds.TopRight))
				element.style.borderTopRightRadius = radius;
			if (cornerKinds.HasFlag(CornerKinds.BottomLeft))
				element.style.borderBottomLeftRadius = radius;
			if (cornerKinds.HasFlag(CornerKinds.BottomRight))
				element.style.borderBottomRightRadius = radius;
		}

		public static void SetAnchoredPosition(this VisualElement element, AnchorKinds anchorKind, Vector2 anchoredPosition)
		{
			StyleLength left = anchorKind.HasFlag(AnchorKinds.Left) ? anchoredPosition.x : StyleKeyword.Auto;
			StyleLength right = anchorKind.HasFlag(AnchorKinds.Right) ? anchoredPosition.x : StyleKeyword.Auto;
			StyleLength top = anchorKind.HasFlag(AnchorKinds.Top) ? anchoredPosition.y : StyleKeyword.Auto;
			StyleLength bottom = anchorKind.HasFlag(AnchorKinds.Bottom) ? anchoredPosition.y : StyleKeyword.Auto;

			element.style.left = left;
			element.style.right = right;
			element.style.top = top;
			element.style.bottom = bottom;
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

		public static void ResetTintColor(this VisualElement element, bool isApplyToChildren = true)
		{
			element.SetTintColor(Color.white);

			if (!isApplyToChildren)
				return;

			foreach (var child in element.Children())
				child.ResetTintColor();
		}
	}
}