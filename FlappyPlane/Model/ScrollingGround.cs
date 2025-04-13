using FlappyPlane.View.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Security.Permissions;

namespace FlappyPlane.Model;

/// <summary>
/// Класс осуществляет прокртуку нижней части Ground
/// </summary>
public class ScrollingGround
{
    public Texture2D Texture
    {
        get { return _texture; }
    }
    private Texture2D _texture;

    public Vector2 Position
    {
        get { return _position; }
    }
    private Vector2 _position;

    private Vector2 _sizeTexture;

    public Color[] DataTexture
    {
        get { return _dataTexture; }
    }
    private Color[] _dataTexture;

    public Rectangle BoundingRectangle
    {
        get { return new Rectangle(
            0, 
            _level.HeightScreen - _texture.Height, 
            _texture.Width, 
            _texture.Height); }
    }

    public Matrix MatrixTransform
    {
        get { return _matrixTransform; }
    }
    private Matrix _matrixTransform;

    public Level Level
    {
        get { return _level; }
    }
    private Level _level;

    public static float Speed { get; set; } = 250.0f;

    public ScrollingGround(Level level)
    {
        _level = level;
        Load();
    }

    private void Load()
    {
        _texture = _level.Content.Load<Texture2D>($"Ground/ground{OptionsMenuScreen.CurrentWorldType}");

        _position = new Vector2(_level.WidthScreen / 2, _level.HeightScreen - _texture.Height);
        _sizeTexture = new Vector2(_texture.Width, 0);

        _dataTexture = new Color[_texture.Width * _texture.Height];
        _texture.GetData(_dataTexture);
    }

    public void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _position.X -= Speed * elapsed;
        if (_position.X <= 0)
            _position.X = _texture.Width;

        var positionPlane = _level.Player.PositionPlane;
       
        if (_position.X <= positionPlane.X + _level.Player.WidthPlane)
            _matrixTransform = Matrix.CreateTranslation(new Vector3(_position, 0.0f));
        else
            _matrixTransform = Matrix.CreateTranslation(new Vector3(_position - _sizeTexture, 0.0f));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_position.X > 0)
            spriteBatch.Draw(_texture, _position, Color.White);

        spriteBatch.Draw(_texture, _position - _sizeTexture, Color.White);
    }
}
