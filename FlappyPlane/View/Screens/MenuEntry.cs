using FlappyPlane.View.GameStateManagment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlappyPlane.View.Screens;

public class MenuEntry
{

    public string Text
    {
        get { return _text; }
        set { _text = value; }
    }
    private string _text;

    private float _selectionFade;

    public Vector2 Position
    {
        get { return _position; }
        set { _position = value; }
    }
    private Vector2 _position;

    public event EventHandler Selected;

    protected internal void OnSelectedEntry()
    {
        Selected?.Invoke(this, EventArgs.Empty);
        MusicManager.PlaybackSoundEffetc("SelectedMenuEntry");
    }

    public MenuEntry(string text)
    {
        _text = text;
    }

    public virtual void Update(GameTime gameTIme, MenuScreen screen, bool isSelected)
    {
        float fadeSpeed = (float)gameTIme.ElapsedGameTime.TotalSeconds * 4;

        if (isSelected)
            _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1);
        else
            _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0);
    }

    public virtual void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
    {
        Color color = isSelected ? ColorPalette.TextColorSelected : ColorPalette.TextColorDefault;

        double time = (float)gameTime.TotalGameTime.TotalSeconds;
        float pulsate = (float)Math.Sin(time * 6) + 1;
        float sclae = 1 + pulsate * 0.05f * _selectionFade;

        var screenManager = screen.ScreenManager;
        var spriteBatch = screenManager.SpriteBatch;
        var font = screenManager.Font;

        var origin = new Vector2(0, font.LineSpacing / 2);

        spriteBatch.DrawString(font, _text, _position, color, 0,
            origin, sclae, SpriteEffects.None, 0);
    }

    public int GetHeight(MenuScreen screen)
    {
        return screen.ScreenManager.Font.LineSpacing;
    }

    public int GetWidth(MenuScreen screen)
    {
        return (int)screen.ScreenManager.Font.MeasureString(_text).X;
    }
}
