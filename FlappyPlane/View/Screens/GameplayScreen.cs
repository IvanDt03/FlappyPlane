using FlappyPlane.Model;
using FlappyPlane.View.GameStateManagment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace FlappyPlane.View.Screens;

public class GameplayScreen : GameScreen
{
    private ContentManager _content;
    private Level _level;

    private Animation _tapAnimation;
    private Texture2D _leftTapTexture;
    private Texture2D _rightTapTexture;

    private bool _isWasStartingGame = false;
    private InputAction _startGameAction;
    
    private float _pauseAlpha;
    private InputAction _pauseAction;

    private Vector2 _positionTap;
    private Vector2 _positionLeftTap;
    private Vector2 _positionRightTap;

    private float _timeSoundNeture = 0.0f;
    private const float IntervalPlayback = 15f;

    public GameplayScreen()
    {
        _pauseAction = new InputAction(
            new Keys[] { Keys.Escape }, true);

        _startGameAction = new InputAction(
            new Keys[] { Keys.Space, Keys.Up, Keys.W }, true);
    }

    public override void Load()
    {
        base.Load();

        if (_content == null)
            _content = new ContentManager(ScreenManager.Game.Services, "Content");

        int width = ScreenManager.Game.GraphicsDevice.Viewport.Width;
        int height = ScreenManager.Game.GraphicsDevice.Viewport.Height;
        _level = new Level(_content, width, height);

        _leftTapTexture = _content.Load<Texture2D>("ForGameplay/tapLeft");
        _rightTapTexture = _content.Load<Texture2D>("ForGameplay/tapRight");

        var taps = new Texture2D[2];
        taps[0] = _content.Load<Texture2D>("ForGameplay/tap");
        taps[1] = _content.Load<Texture2D>("ForGameplay/tapTick");
        _tapAnimation = new Animation(taps, 0.3f);

        _positionTap = new Vector2(
            _level.Player.PositionPlane.X + _level.Player.WidthPlane,
            _level.Player.PositionPlane.Y + _level.Player.HeightPlane);

        _positionLeftTap = new Vector2(
            _level.Player.PositionPlane.X + _level.Player.WidthPlane * 1.2f,
            _level.Player.PositionPlane.Y + _level.Player.HeightPlane * 0.3f);

        _positionRightTap = new Vector2(
            _level.Player.PositionPlane.X - _level.Player.WidthPlane * 1.2f,
            _level.Player.PositionPlane.Y + _level.Player.HeightPlane * 0.3f);

        
    }

    public override void Unload()
    {
        _content.Unload();
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        base.Update(gameTime, otherScreenHasFocus, false);

        if (coveredByOtherScreen)
            _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
        else
            _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);


        if (IsActive && _isWasStartingGame)
        {
            _level.Update(gameTime, ScreenManager.InputState);
            _timeSoundNeture += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (MediaPlayer.State == MediaState.Stopped)
                MusicManager.PlaybackSong("Wind");

            if (!_level.Player.IsAlive)
            {
                ScreenManager.AddScreen(new GameOverScreen(_level.Score));
                MediaPlayer.Stop();
            }

            if (_timeSoundNeture >= IntervalPlayback)
            {
                MusicManager.PlaybackSoundEffetc("Bird");
                _timeSoundNeture = 0.0f;
            }
        }
    }

    public override void HandleInput(GameTime gameTime, InputState input)
    {
        if (input == null)
            throw new ArgumentNullException("input");

        if (_pauseAction.Evaluate(input))
        {
            ScreenManager.AddScreen(new PauseScreen());
            MediaPlayer.Stop();
        }

        if (_startGameAction.Evaluate(input) && !_isWasStartingGame)
            _isWasStartingGame = true;
    }

    public override void Draw(GameTime gameTime)
    {
        var spriteBatch = ScreenManager.SpriteBatch;

        spriteBatch.Begin();

        _level.Draw(gameTime, spriteBatch);

        if (IsActive)
        {
            spriteBatch.DrawString(ScreenManager.Font, $"Score: {_level.Score}",
                new Vector2(_level.WidthScreen / 2, _level.HeightScreen * 0.2f),
                ColorPalette.TextColorDefault,
                0.0f,
                ScreenManager.Font.MeasureString($"Score: {_level.Score}") / 2,
                1.0f,
                SpriteEffects.None,
                0.0f);
        }

        if (!_isWasStartingGame)
        {
            _tapAnimation.Draw(gameTime, spriteBatch, _positionTap, _level.Player.Origin, 0.0f);
            spriteBatch.Draw(_leftTapTexture, _positionLeftTap, null, Color.White, 0.0f, _level.Player.Origin, 1f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(_rightTapTexture, _positionRightTap, null, Color.White, 0.0f, _level.Player.Origin, 1f, SpriteEffects.None, 0.0f);
        }

        spriteBatch.End();

        if (_pauseAlpha > 0)
        {
            float alpha = MathHelper.Lerp(0f, 1f, _pauseAlpha / 2);

            ScreenManager.FadeBackBufferToBlack(alpha);
        }
    }
}
