using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyPlane.View.Screens;

public class VolumeMenuEntry : MenuEntry
{
    private Slider _slider;

    public VolumeMenuEntry(string text, GraphicsDevice device) : base(text)
    {
        _slider = new Slider(device);
    }

    public override void Update(GameTime gameTIme, MenuScreen screen, bool isSelected)
    {
        base.Update(gameTIme, screen, isSelected);
    }

    public override void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
    {
        base.Draw(gameTime, screen, isSelected);

        _slider.Draw(screen, this);
    }
} 
