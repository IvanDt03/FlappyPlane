using FlappyPlane.View.GameStateManagment;
using FlappyPlane.View.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;



namespace FlappyPlane.Model;

public class Player
{
    #region Fields

    private InputAction _actionJump;

    // Необходимые поля для анимации спрайтов персонажа
    private Animation _plane;
    private Animation _puff;

    public Vector2 Origin { get; private set; }

    // Поля для отслеживания состояния Plane
    public Vector2 PositionPlane
    {
        get { return _positionPlane; }
    }
    private Vector2 _positionPlane;

    public Rectangle BoundingRectangle 
        => CalculateBoundingRectangle(
            new Rectangle(0, 0, _plane.Texture.Width, 
                _plane.Texture.Height), _matrixTransform);

    public int WidthPlane => _plane.Texture.Width;
    public int HeightPlane => _plane.Texture.Height;

    public Matrix MatrixTransform
    {
        get { return _matrixTransform; }
    }
    private Matrix _matrixTransform;

    public Color[] DataTexturePlane
    {
        get { return _dataTexturePlane; }
    }
    private Color[] _dataTexturePlane;

    public bool IsAlive { get; set; } = true;

    // Поля для отслеживания состояния Puff
    public Vector2 PositionPuff
    {
        get { return _positionPuff; }
    }
    private Vector2 _positionPuff;
    private Vector2 _positionPuffToDraw;

    // Физические данные 
    private float _rotationAngle    = 0.0f;
    private Vector2 _velocity       = Vector2.Zero;

    public Level Level
    {
        get { return _level; }
    }
    private Level _level;

    // Константы для перемещения персонажа
    private const float MoveSpeed           = 0.0f;
    private const float GravityAcceleration = 380f;
    private const float JumpLaunchVelocity  = -120f;
    private const float JumpControlPower    = 0.15f;

    // Поля для отслеживания состояния прыжка персонажа
    private bool _isJumping             = false;
    private bool _wasJumping            = false;
    private float _jumpTime             = 0.0f;
    private const float MaxJumpTime     = 0.11f;
    

    #endregion

    #region LoadContent
    public Player(Level level, Vector2 positionPlane)
    {
        _actionJump = new InputAction(
            new Keys[] { Keys.W, Keys.Up, Keys.Space }, false);

        _level = level;
        LoadContent(positionPlane);
    } 

    public void LoadContent(Vector2 positionPlane)
    {
        var texturesPlane = new Texture2D[3];
        for (int i = 0; i < texturesPlane.Length; i++)
            texturesPlane[i] = 
                _level.Content.Load<Texture2D>($"Planes/{OptionsMenuScreen.CurrentColorPlane}/plane{OptionsMenuScreen.CurrentColorPlane}{i + 1}");

        var texturesPuff = new Texture2D[2];
        texturesPuff[0] = _level.Content.Load<Texture2D>("Puffs/puffLarge");
        texturesPuff[1] = _level.Content.Load<Texture2D>("Puffs/puffSmall")
            .ConversionTexture(texturesPuff[0].GraphicsDevice, texturesPuff[0]);

        _plane = new Animation(texturesPlane, 0.1f);
        _puff = new Animation(texturesPuff, 0.3f);
        Origin = new Vector2(_plane.Texture.Width / 2, _plane.Texture.Height / 2);

        _positionPlane = positionPlane;
        _positionPuff = new Vector2(positionPlane.X - _puff.Texture.Width * 0.8f,
            positionPlane.Y + _plane.Texture.Height - _puff.Texture.Height);
        _positionPuffToDraw = _positionPuff;

        _dataTexturePlane = new Color[_plane.Texture.Width * _plane.Texture.Height];
        _plane.Texture.GetData(_dataTexturePlane);
    }

    #endregion

    #region Update

    public void Update(GameTime gameTime, InputState state)
    {

        _isJumping = _actionJump.Evaluate(state);

       ApplyPhisics(gameTime);

        if (_velocity.Y < 0)
            _rotationAngle = -0.25f;
        else
        {
            _rotationAngle += 0.04f;
            _rotationAngle = MathHelper.Clamp(_rotationAngle, -0.2f, 0.9f);
        }

        _positionPlane += _velocity;
        _positionPuff += _velocity;

        _matrixTransform =
            Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            Matrix.CreateRotationZ(_rotationAngle) *
            Matrix.CreateTranslation(new Vector3(_positionPlane, 0.0f));

        _positionPuffToDraw = RotatePoint(_positionPlane, _rotationAngle, _positionPuff);
    }

    public void OnDied()
    {
        IsAlive = false;
        _plane.Pause = true;
        _puff.Pause = true;
        MusicManager.PlaybackSoundEffetc("PlayerDie");
    }

    private Vector2 RotatePoint(Vector2 pointOfRotate, float rotation, Vector2 position)
    {
        Matrix matrix = Matrix.CreateRotationZ(rotation);
        Vector2 rotatedVector = Vector2.Transform(position - pointOfRotate, matrix);
        return rotatedVector + pointOfRotate;
    }

    private void ApplyPhisics(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _velocity.X = MoveSpeed * elapsed;
        _velocity.Y = GravityAcceleration * elapsed;

        // В случае, если игрок находится за пределами карты 
        // то отменяем действие пражка
        if (_positionPlane.Y + BoundingRectangle.Height / 2 > 0)
            _velocity.Y = DoJump(_velocity.Y, elapsed);
    }

    private float DoJump(float velocityY, float elapsed)
    {
        if (_isJumping || _wasJumping)
        {
            if (_jumpTime == 0.0f)
                MusicManager.PlaybackSoundEffetc("PlayerJump");

            _jumpTime += elapsed;

            if (0.0f < _jumpTime && _jumpTime <= MaxJumpTime)
            {
                velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(_jumpTime / MaxJumpTime, JumpControlPower));
                _wasJumping = true;
            }
            else
                _wasJumping = false;
        }
        else
            _jumpTime = 0.0f;

        return velocityY;
    }

    // Так как наш самолет имеет способность вращаться 
    // необходимо возвращать гарницы текстуры на основании угла ротации и перемещения 
    private Rectangle CalculateBoundingRectangle(Rectangle source, Matrix transform)
    {
        Vector2 leftTop = new Vector2(source.Left, source.Top);
        Vector2 rightTop = new Vector2(source.Right, source.Top);
        Vector2 leftBottom = new Vector2(source.Left, source.Bottom);
        Vector2 rightBottom = new Vector2(source.Right, source.Bottom);

        Vector2.Transform(ref leftTop, ref transform, out leftTop);
        Vector2.Transform(ref rightTop, ref transform, out rightTop);
        Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
        Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

        Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
        Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                  Vector2.Max(leftBottom, rightBottom));

        return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
    }

    #endregion

    #region Draw
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _plane.Draw(gameTime, spriteBatch, _positionPlane, Origin, _rotationAngle);
        _puff.Draw(gameTime, spriteBatch, _positionPuffToDraw, Origin, _rotationAngle);
    }

    #endregion
}
