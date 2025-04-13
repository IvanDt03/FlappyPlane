using FlappyPlane.View.GameStateManagment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace FlappyPlane.View.Screens;

public class Slider
{
    private Texture2D _barTexture;
    private Texture2D _knobTexture;

    public Vector2 Position
    {
        get { return  _barPosition; }
    }
    private Vector2 _barPosition;

    private Vector2 _knobPosition = Vector2.Zero;

    public Slider(GraphicsDevice device)
    {
        _barTexture = CreateTexture(device, 100, 20);
        _knobTexture = CreateTexture(device, 10, 20);
    }

    private void UpdatePositionSlider(MenuScreen screen, MenuEntry entry)
    {
        _barPosition = new Vector2(entry.Position.X + entry.GetWidth(screen) * 1.2f, entry.Position.Y);


        float volume = entry.Text.Contains("General") ? MusicManager.MusicVolumeMenu : MusicManager.MusicVolumeEffects;
        _knobPosition
            = new Vector2(
                volume * (_barTexture.Width - _knobTexture.Width) + _barPosition.X,
                _barPosition.Y);
    }

    public void Draw(MenuScreen screen, MenuEntry entry)
    {
        var spriteBatch = screen.ScreenManager.SpriteBatch;
        var origin = new Vector2(0, screen.ScreenManager.Font.LineSpacing / 2);

        UpdatePositionSlider(screen, entry);

        spriteBatch.Draw(_barTexture, _barPosition, null, ColorPalette.BarSliderColor * 0.7f, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        spriteBatch.Draw(_knobTexture, _knobPosition, null, ColorPalette.KnobSliderColor * 1f, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
    }


    private Texture2D CreateTexture(GraphicsDevice device, int width, int height)
    {
        Texture2D result = new Texture2D(device, width, height, false, SurfaceFormat.Color);

        Color[] colors = new Color[result.Width * result.Height];
        for(int i = 0; i < colors.Length; i++)
            colors[i] = Color.White;

        result.SetData(colors);

        return result;
    }
}
