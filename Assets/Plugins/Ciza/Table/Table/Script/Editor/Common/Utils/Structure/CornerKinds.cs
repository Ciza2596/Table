using System;

namespace CizaTable.Editor
{
    [Flags]
    public enum CornerKinds
    {
        TopLeft = 1 << 0,
        TopRight = 1 << 1,
        BottomLeft = 1 << 2,
        BottomRight = 1 << 3,

        All = TopLeft | TopRight | BottomLeft | BottomRight,
        TopLeftAndBottomRight = TopLeft | BottomRight,
        TopRightAndBottomLeft = TopRight | BottomLeft,

        Top = TopLeft | TopRight,
        Bottom = BottomLeft | BottomRight,
        Left = TopLeft | BottomLeft,
        Right = TopRight | BottomRight,

        NoTopLeft = All ^ TopLeft,
        NoTopRight = All ^ TopRight,
        NoBottomLeft = All ^ BottomLeft,
        NoBottomRight = All ^ BottomRight
    }
}
