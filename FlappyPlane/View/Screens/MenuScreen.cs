using FlappyPlane.View.GameStateManagment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace FlappyPlane.View.Screens;

public class MenuScreen : GameScreen
{
    private List<MenuEntry> _entries = new List<MenuEntry>();
    protected int selectedEntry = 0;
    private string _menuTitle;

    private InputAction _menuUp;
    private InputAction _menuDown;
    private InputAction _menuSelect;
    private InputAction _menuCancel;

    protected IList<MenuEntry> MenuEntries
    {
        get { return _entries; }
    }

    public MenuScreen(string menuTitle)
    {
        _menuTitle = menuTitle;

        _menuUp = new InputAction(
            new Keys[] { Keys.Up, Keys.W }, true);
        _menuDown = new InputAction(
            new Keys[] { Keys.Down, Keys.S }, true);
        _menuSelect = new InputAction(
            new Keys[] { Keys.Enter, Keys.Space }, true);
        _menuCancel = new InputAction(
            new Keys[] { Keys.Escape }, true);
    }

    public override void HandleInput(GameTime gameTime, InputState keyState)
    {
        if (_menuUp.Evaluate(keyState))
        {
            selectedEntry--;

            if (selectedEntry < 0)
                selectedEntry = _entries.Count - 1;

            MusicManager.PlaybackSoundEffetc("MoveMenuEntry");
        }
        
        if (_menuDown.Evaluate(keyState))
        {
            selectedEntry++;

            if (selectedEntry >= _entries.Count)
                selectedEntry = 0;

            MusicManager.PlaybackSoundEffetc("MoveMenuEntry");
        }

        if (_menuSelect.Evaluate(keyState))
            OnSelected(selectedEntry);
        else if (_menuCancel.Evaluate(keyState))
            OnCancel();
    }

    protected virtual void OnSelected(int indexSelected)
    {
        _entries[indexSelected].OnSelectedEntry();
    }

    protected virtual void OnCancel()
    {
        ExitScreen();
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        for (int i = 0; i < _entries.Count; i++)
        {
            bool isSelected = IsActive && (i == selectedEntry);
            _entries[i].Update(gameTime, this, isSelected);
        }
    }

    public override void Draw(GameTime gameTime)
    {

        UpdateMenuEntryLocations(gameTime);

        var graphics = ScreenManager.GraphicsDevice;
        var spriteBatch = ScreenManager.SpriteBatch;
        var font = ScreenManager.Font;

        spriteBatch.Begin();

        for (int i = 0; i < _entries.Count; ++i)
        {
            bool isSelected = IsActive && (i == selectedEntry);
            _entries[i].Draw(gameTime,this, isSelected);
        }

        Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height * 0.2f);
        Vector2 titleOrigin = font.MeasureString(_menuTitle) / 2;
        float titleScale = 2f;

        spriteBatch.DrawString(font, _menuTitle, titlePosition, ColorPalette.TextColorDefault, 0,
                               titleOrigin, titleScale, SpriteEffects.None, 0);

        spriteBatch.End();
    }

    protected virtual void UpdateMenuEntryLocations(GameTime gameTIme)
    {
        Vector2 position = new Vector2(0f, ScreenManager.Game.GraphicsDevice.Viewport.Height / 2);

        for (int i = 0; i < _entries.Count; ++i)
        {
            var entry = _entries[i];

            position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - entry.GetWidth(this) / 2;
        
            entry.Position = position;
            position.Y += entry.GetHeight(this) * 1.5f;
        }
    }
}
