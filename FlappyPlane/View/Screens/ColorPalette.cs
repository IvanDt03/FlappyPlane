using Microsoft.Xna.Framework;

namespace FlappyPlane.View.Screens;

public enum Theme
{
    Light,
    Dark,
}
public static class ColorPalette
{
    public static Theme Theme { get; set; } = Theme.Light;
    public static Color TextColorDefault
    {
        get
        {
            return Theme == Theme.Light ? Color.Black : Color.GhostWhite;
        }
    }

    public static Color TextColorSelected
    {
        get
        {
            return Theme == Theme.Light ? Color.Red : Color.Yellow;
        }
    }
    

    public static Color BackgroundColor
    {
        get
        {
            return Theme == Theme.Light ? Color.Bisque : Color.SlateGray;
        }
    }

    public static Color MenuPauseColor
    {
        get
        {
            return Theme == Theme.Light ? Color.LightGray : Color.DarkSlateGray;
        }
    }

    public static Color BarSliderColor
    {
        get
        {
            return Theme == Theme.Light ? Color.RosyBrown : Color.Ivory;
        }
    }

    public static Color KnobSliderColor
    {
        get
        {
            return Theme == Theme.Light ? Color.Black : Color.Black;
        }
    }
}
