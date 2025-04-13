using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FlappyPlane.Model;

public class Coin
{
    public Texture2D Texture
    {
        get { return _texture; }
    }
    private Texture2D _texture;

    public bool IsActive { get; set; }

    public int Value { get; } = 20;

    public Vector2 Position
    {
        get { return _position; }
    }
    private Vector2 _position;

    private Vector2 _origin;

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

    public Rectangle BoundingRectangle
    {
        get { return new Rectangle(
            (int)(_position.X - _origin.X), 
            (int)(_position.Y - _origin.Y),
            _texture.Width, 
            _texture.Height); }
    }

    public Level Level
    {
        get { return _level; }
    }
    private Level _level;

    public static float Speed { get; set; } = 250.0f;

    public Coin(Level level)
    {
        _level = level;
        IsActive = false;

        Load();
    }

    private void Load()
    {
        _texture = _level.Content.Load<Texture2D>("Coins/starGold");

        _dataTexture = new Color[_texture.Width * _texture.Height];
        _texture.GetData(_dataTexture);
        _origin = new Vector2(_texture.Width / 2.0f, _texture.Height / 2.0f);
    }

    public void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (IsActive)
        {
            _position.X -= Speed * elapsed;
            _matrixTransform =
                Matrix.CreateTranslation(new Vector3(-_origin, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(_position, 0.0f));
            if (IsOffScreen())
                IsActive = false;
        }
    }

    public void Activate(Vector2 position)
    {
        _position = position;
        IsActive = true;
    }

    private bool IsOffScreen()
    {
        return _position.X - _origin.X + _texture.Width < 0;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsActive)
            spriteBatch.Draw(_texture, _position, null, Color.White, 0.0f, _origin, 1.0f, SpriteEffects.None, 0.0f);
    }
}
