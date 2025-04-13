using FlappyPlane.View.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FlappyPlane.View.GameStateManagment;

public class ScreenManager : DrawableGameComponent
{
    private List<GameScreen> _screens = new List<GameScreen>();
    private List<GameScreen> _screensToUpdate = new List<GameScreen>();
    private bool _isInitialized;

    public InputState InputState
    {
        get { return _input; }
        private set { _input = value; }
    }
    private InputState _input = new InputState();

    public Texture2D BlankTexture
    {
        get { return _textureBlank; }
    }
    private Texture2D _textureBlank;

    public SpriteBatch SpriteBatch
    {
        get { return _spriteBatch; }
    }
    private SpriteBatch _spriteBatch;

    public SpriteFont Font
    {
        get { return _font; }
    }
    private SpriteFont _font;

    public ScreenManager(FlappyPlaneGame game) : base(game) { }

    public override void Initialize()
    {
        base.Initialize();
        _isInitialized = true;
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        var content = Game.Content;
        _font = content.Load<SpriteFont>("Fonts/gamefont");
        _textureBlank = content.Load<Texture2D>("blank");

        foreach (var screen in _screens)
            screen.Load();
    }

    protected override void UnloadContent()
    {
        foreach (var screen in _screens)
            screen.Unload();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _input.Update();

        _screensToUpdate.Clear();

        foreach (var screen in _screens)
            _screensToUpdate.Add(screen);

        bool otherScreenHasFocus = !Game.IsActive;
        bool coveredByOtherScreen = false;

        while (_screensToUpdate.Count > 0)
        {
            var screen = _screensToUpdate[_screensToUpdate.Count - 1];
            _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

            screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

           if (screen.ScreenState == ScreenState.Active)
            {
                if (!otherScreenHasFocus)
                {
                    screen.HandleInput(gameTime, _input);
                    otherScreenHasFocus = true;
                }

                coveredByOtherScreen = true;
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        foreach (var screen in _screens)
        {
            if (screen.ScreenState == ScreenState.Hidden)
                continue;
            screen.Draw(gameTime);
        }
    }

    public void AddScreen(GameScreen screen)
    {
        screen.ScreenManager = this;

        if (_isInitialized)
            screen.Load();

        _screens.Add(screen);
    }

    public void RemoveScreen(GameScreen screen)
    {
        if (_isInitialized)
            screen.Unload();

        _screens.Remove(screen);
        _screensToUpdate.Remove(screen);
    } 

    public GameScreen[] GetScreens()
    {
        return _screens.ToArray();
    }

    public void FadeBackBufferToBlack(float alpha)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_textureBlank, GraphicsDevice.Viewport.Bounds, ColorPalette.MenuPauseColor * alpha);
        _spriteBatch.End();
    }
}
