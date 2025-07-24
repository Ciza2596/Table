using UnityEngine;
using UnityEngine.Scripting;

namespace CizaTable.Editor
{
    public class ColorSet
    {
        public Color BackgroundColor { get; }
        public Color BorderColor { get; }
        public Color TextColor { get; }

        public Color HoverStateMultiplier { get; } = new Color(1.1f, 1.1f, 1.1f, 1);
        public Color ActiveStateMultiplier { get; } = new Color(0.95f, 0.95f, 0.95f, 1);
        public Color DisableStateMultiplier { get; } = new Color(1, 1, 1, 0.5f);
        
        public Color TintMultiplier { get; set; } = new Color(1, 1, 1, 1);
        public ColorSet TintSet => this * TintMultiplier;
        public ColorSet HoverSet => this * HoverStateMultiplier * TintMultiplier;
        public ColorSet ActiveSet => this * ActiveStateMultiplier * TintMultiplier;
        public ColorSet DisabledSet => this * DisableStateMultiplier * TintMultiplier;

        [Preserve]
        public ColorSet(Color backgroundColor, Color borderColor, Color textColor, Color hoverStateMultiplier = default, Color activeStateMultiplier = default, Color disabledStateMultiplier = default)
        {
            BackgroundColor = backgroundColor;
            BorderColor = borderColor;
            TextColor = textColor;

            if (hoverStateMultiplier != default)
                HoverStateMultiplier = hoverStateMultiplier;
            if (activeStateMultiplier != default)
                ActiveStateMultiplier = activeStateMultiplier;
            if (disabledStateMultiplier != default)
                DisableStateMultiplier = disabledStateMultiplier;
        }

        public static ColorSet operator *(ColorSet colorSet, Color multiplier) => new(colorSet.BackgroundColor * multiplier, colorSet.BorderColor * multiplier, colorSet.TextColor * multiplier);
        public static ColorSet operator *(ColorSet colorSet, float multiplier) => new(colorSet.BackgroundColor * multiplier, colorSet.BorderColor * multiplier, colorSet.TextColor * multiplier);
    }
}
