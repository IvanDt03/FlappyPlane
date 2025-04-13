using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyPlane.View.Screens;

public class GameOverScreen : MenuScreen
{
    private ContentManager _content;
    private Texture2D _textureGameOver;
    private Vector2 _positionTextGameOver;
    private Vector2 _originTexture;
    private int _score;

    public GameOverScreen(int score) : base("") 
    {
        MenuEntry entryRestart = new MenuEntry("Restart");
        MenuEntry entryQuit = new MenuEntry("Quit");

        entryRestart.Selected += EntryRestart_Selected;
        entryQuit.Selected += EntryQuit_Selected;

        MenuEntries.Add(entryRestart);
        MenuEntries.Add(entryQuit);

        _score = score;
    }

    private void EntryQuit_Selected(object sender, System.EventArgs e)
    {
        OnCancel();
    }

    private void EntryRestart_Selected(object sender, System.EventArgs e)
    {
        foreach (var screen in ScreenManager.GetScreens())
        {
            if (screen is BackgroundSceen)
                continue;
            screen.ExitScreen();
        }
        ScreenManager.AddScreen(new GameplayScreen());
    }

    public override void Load()
    {
        if (_content == null)
            _content = new ContentManager(ScreenManager.Game.Services, "Content");

        _textureGameOver = _content.Load<Texture2D>("textGameOver");
        
        int width = ScreenManager.Game.GraphicsDevice.Viewport.Width;
        int height = ScreenManager.Game.GraphicsDevice.Viewport.Height;


        _positionTextGameOver = new Vector2(width / 2, height * 0.2f);
        _originTexture = new Vector2(_textureGameOver.Width / 2, 0);
    }

    protected override void OnCancel()
    {
        foreach (var screen in ScreenManager.GetScreens())
        {
            if (screen is BackgroundSceen)
                continue;
            screen.ExitScreen();
        }
        ScreenManager.AddScreen(new MainManuScreen());
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        var spriteBatch = ScreenManager.SpriteBatch;

        spriteBatch.Begin();

        spriteBatch.Draw(_textureGameOver, _positionTextGameOver, null,
            Color.White, 0.0f, _originTexture, 1.0f, SpriteEffects.None, 0.0f);

        var font = ScreenManager.Font;
        string textScore = $"Your score: {_score}";
        Vector2 originText = _originTexture + new Vector2(font.MeasureString(textScore).X / 2, 0);
        Vector2 positionText = new Vector2(_positionTextGameOver.X + _textureGameOver.Width / 2,
            _positionTextGameOver.Y +  _textureGameOver.Height);
        
        spriteBatch.DrawString(font, textScore, positionText, ColorPalette.TextColorDefault,
            0.0f,originText, 1.0f, SpriteEffects.None, 0.0f);

        spriteBatch.End();
    }

    public override void Unload()
    {
        _content.Unload();
    }
}
