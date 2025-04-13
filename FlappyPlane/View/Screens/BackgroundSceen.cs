using FlappyPlane.View.GameStateManagment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyPlane.View.Screens;

public class BackgroundSceen : GameScreen
{
    private ContentManager _content;
    private Texture2D _textute;

    public BackgroundSceen() { }

    public override void Load()
    {
        if (_content == null)
            _content = new ContentManager(ScreenManager.Game.Services, "Content");

        _textute = _content.Load<Texture2D>("background");
    }

    public override void Unload()
    {
        _content.Unload();
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        base.Update(gameTime, otherScreenHasFocus, false);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        var spriteBatch = ScreenManager.SpriteBatch;
        var viewport = ScreenManager.GraphicsDevice.Viewport;
        var rect = new Rectangle(0, 0, viewport.Width, viewport.Height);

        spriteBatch.Begin();
        spriteBatch.Draw(_textute, rect, ColorPalette.BackgroundColor);
        spriteBatch.End();
    }
}
