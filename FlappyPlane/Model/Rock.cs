using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlappyPlane.Model;

public class Rock 
{
    #region Fields

    public Texture2D Texture
    {
        get { return _texture; }
    }
    private Texture2D _texture;

    public bool IsActive { get; private set; }

    public static float Speed { get; set; } = 250.0f;

    public Vector2 Position
    {
        get { return _position; }
    }
    private Vector2 _position;

    public Rectangle BoundingRectangle
    {
        get { return new Rectangle(
            (int)_position.X, 
            (int)_position.Y, 
            _texture.Width, 
            _texture.Height); }
    }

    public Matrix MatrixTransform
    {
        get { return _matrixTransform; }
    }
    private Matrix _matrixTransform;

    public Color[] DataTexture
    {
        get { return _dataTexture; }
    }
    private Color[] _dataTexture;

    public bool RockIsUp
    {
        get { return _rockIsUp; }
    }
    private bool _rockIsUp;

    public Level Level
    {
        get { return _level; }
    }
    private Level _level;

    private const float CorrectionFactorInPixels = 10.0f;

    private Random _rand = new Random();

    #endregion

    #region Load Content
    public Rock(Level level, bool rockIsUp)
    {
        _level      = level;
        _rockIsUp   = rockIsUp;
        IsActive    = false;

        Load();
    }

    private void Load()
    {
        var listRocks = _level.RockNameList;
        var randomRock = listRocks[_rand.Next(listRocks.Length)];
        if (_rockIsUp)
            _texture = _level.Content.Load<Texture2D>($"RockUp/{randomRock}Up");
        else
            _texture = _level.Content.Load<Texture2D>($"RockDown/{randomRock}Down");

        _dataTexture = new Color[_texture.Width * _texture.Height];
        _texture.GetData(_dataTexture);
    }

    #endregion

    #region Update
    public void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (IsActive)
        {
            _position.X -= Speed * elapsed;
            _matrixTransform = Matrix.CreateTranslation(new Vector3(_position, 0.0f));
            if (IsOffScreen())
                IsActive = false;
        }
    }

    public void Activate(float randomOffsetByY)
    {
        if (_rockIsUp)
            _position = new Vector2(_level.WidthScreen + 100, 0 - CorrectionFactorInPixels - randomOffsetByY);
        else
            _position = new Vector2(_level.WidthScreen + 100, _level.HeightScreen / 2 + CorrectionFactorInPixels + randomOffsetByY);

        IsActive = true;
    }

    private bool IsOffScreen()
    {
        return _position.X + _texture.Width < 0;
    }

    #endregion 

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive)
            spriteBatch.Draw(_texture, _position, Color.White);
    }
}