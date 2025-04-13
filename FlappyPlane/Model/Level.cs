using FlappyPlane.View.GameStateManagment;
using FlappyPlane.View.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappyPlane.Model;

public class Level
{
    #region Fields

    // Данные представления для генерации позиций сущностей 
    public readonly int WidthScreen;
    public readonly int HeightScreen;

    // Физические структуры уровня
    private ScrollingGround _ground;
    private Dictionary<string, List<string>> _rocksForGround = new Dictionary<string, List<string>>();

    public ReadOnlySpan<string> RockNameList 
        => new ReadOnlySpan<string>(_rocksForGround[OptionsMenuScreen.CurrentWorldType].ToArray());

    // Сущности 
    private List<Rock> _rocks;
    private List<Coin> _coins;

    public Player Player
    {
        get { return _player; }
    }
    private Player _player;

    // Состояние игрового процесса
    public ContentManager Content
    {
        get { return _content; }
    }
    private ContentManager _content;

    private readonly Random _rand = new Random();

    public int Score
    {
        get { return _score; }
    }
    private int _score;

    private Rock _previousRock;

    // Константы для контроля за количеством сущностей
    private const int CountCoins = 15;
    private const int CountRocks = 10;

    // Поля для отслеживания поялвния сущностей
    private float _timeSpawn            = 0.0f;
    private float _timeSpawnFactor      = 3.5f;
    private const float _IntervalSpaw   = 1.7f; 

    // Поля для управления сложностью
    private const float _IntervalDifficulty = 10.0f;
    private float _timeDifficulty           = 0.0f;

    #endregion

    #region LoadContent

    public Level(ContentManager content, int width, int height)
    {
        WidthScreen = width;
        HeightScreen = height;

        _content = content;

        _ground = new ScrollingGround(this);

        LoadRocksForGround();

        LoadEntities();
    }

    private void LoadRocksForGround()
    {
        _rocksForGround["Grass"] = new List<string> { "rock", "rockGrass" };
        _rocksForGround["Ice"] = new List<string> { "rockIce", "rockSnow" };
        _rocksForGround["Rock"] = new List<string> { "rock", "rockGrass", "rockSnow" };
        _rocksForGround["Snow"] = new List<string> { "rockIce", "rock", "rockSnow" };
        _rocksForGround["Dirt"] = new List<string> { "rock", "rockGrass", "rockSnow" };
    }

    private void LoadEntities()
    {

        _player = new Player(this, new Vector2(WidthScreen * 0.2f, HeightScreen * 0.5f));
        _coins = new List<Coin>(CountCoins);
        _rocks = new List<Rock>(CountRocks);

        Rock.Speed = 250.0f;
        Coin.Speed = 250.0f;
        ScrollingGround.Speed = 250.0f;

        for (int i = 0; i < CountCoins; ++i)
            _coins.Add(new Coin(this));

        bool rockIsUp = true;
        for(int i = 0; i < CountRocks; ++i)
        {
            _rocks.Add(new Rock(this, rockIsUp));
            rockIsUp = !rockIsUp;
        }
    }

    #endregion

    #region Update

    public void Update(GameTime gameTime, InputState state)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        ChangesDifficulty(elapsed);

        _timeSpawn += MathHelper.Clamp(_rand.NextSingle(), 0.1f, 0.9f) * _timeSpawnFactor * elapsed;

        if (_timeSpawn >= _IntervalSpaw)
        {
            var rock = GetInactiveRock();
            var coin = _coins.FirstOrDefault(c => !c.IsActive);

            if (rock != null)
            {
                float randomOffsetByY = rock.Texture.Height * _rand.NextSingle(0.15f, 0.3f);
                rock.Activate(randomOffsetByY);

                var positionCoin = GenerateRandomPositionCoin(rock, coin);
                coin?.Activate(positionCoin);

                _previousRock = rock;
            }
            _timeSpawn = 0.0f;
        }

        _player.Update(gameTime, state);
        UpdateGround(gameTime);
        UpdateRocks(gameTime);
        UpdateCoins(gameTime);
    }

    private void UpdateGround(GameTime gameTime)
    {
        _ground.Update(gameTime);
        if (_ground.BoundingRectangle.Intersects(_player.BoundingRectangle))
        {
            var matrixPlane     = _player.MatrixTransform;
            var widthPlane      = _player.WidthPlane;
            var heightPlane     = _player.HeightPlane;
            var dataPlane       = _player.DataTexturePlane;

            var matrixGround    = _ground.MatrixTransform;
            var widthGround     = _ground.Texture.Width;
            var heightGround    = _ground.Texture.Height;
            var dataGround      = _ground.DataTexture;

            if (IntersectPixels(
                matrixPlane, widthPlane, heightPlane, dataPlane,
                matrixGround, widthGround, heightGround, dataGround))
                _player.OnDied();
        }
    }
    private void UpdateCoins(GameTime gameTime)
    {
        for (int i = 0; i < CountCoins; ++i)
        {
            Coin coin = _coins[i];
            coin.Update(gameTime);

            if (coin.IsActive && coin.BoundingRectangle.Intersects(_player.BoundingRectangle))
            {
                var matrixPlane = _player.MatrixTransform;
                var widthPlane  = _player.WidthPlane;
                var heightPlane = _player.HeightPlane;
                var dataPlane   = _player.DataTexturePlane;

                var matrixCoin  = coin.MatrixTransform;
                var widthCoin   = coin.Texture.Width;
                var heightCoin  = coin.Texture.Height;
                var dataCoin    = coin.DataTexture;

                if (IntersectPixels(
                    matrixPlane, widthPlane, heightPlane, dataPlane,
                    matrixCoin, widthCoin, heightCoin, dataCoin))
                {
                    _score += coin.Value;
                    coin.IsActive = false;
                    MusicManager.PlaybackSoundEffetc("CoinCollected");
                }
            }
        }
    } 

    private void UpdateRocks(GameTime gameTime)
    {
        foreach (var rock in _rocks)
        {
            rock.Update(gameTime);

            if (rock.IsActive && rock.BoundingRectangle.Intersects(_player.BoundingRectangle))
            {
                var matrixPlane = _player.MatrixTransform;
                var widthPlane  = _player.WidthPlane;
                var heightPlane = _player.HeightPlane;
                var dataPlane   = _player.DataTexturePlane;

                var matrixRock  = rock.MatrixTransform;
                var widthRock   = rock.Texture.Width;
                var heightRock  = rock.Texture.Height;
                var dataRock    = rock.DataTexture;

                if (IntersectPixels(
                    matrixPlane, widthPlane, heightPlane, dataPlane,
                    matrixRock, widthRock, heightRock, dataRock))
                {
                    _player.OnDied();
                }

            }
                
        }
    }
    
    private void ChangesDifficulty(float elapsed)
    {
        _timeDifficulty += elapsed;
        if (_timeDifficulty >= _IntervalDifficulty)
        {
            Rock.Speed  = MathHelper.Clamp(Rock.Speed + 10, 250, 550);
            ScrollingGround.Speed = MathHelper.Clamp(ScrollingGround.Speed + 10, 250, 550);
            Coin.Speed = MathHelper.Clamp(Coin.Speed + 10, 250, 550);
            _timeSpawnFactor = MathHelper.Clamp(_timeSpawnFactor + 0.1f, 3.5f, 10f);
            _timeDifficulty = 0.0f;
        }
    }

    private Vector2 GenerateRandomPositionCoin(Rock rock, Coin coin)
    {
        // Выбирается случайная координата Х для расположения монеты отсносительно камня
        float coinX = 
            (rock.Position.X + rock.Texture.Width / 2) + _rand.NextSingle(-rock.Texture.Width, rock.Texture.Width) * 0.8f;

        float coinY;
        float origin = coin.Texture.Height / 2; // Необходимо учитывать смещение монеты, чтобы она не появлялась на камне

        if (rock.RockIsUp)
        {
            // Если камень является верхним, то монета будет расположена между верхним камнем и фоном ScrollingGround
            float bottomRock = rock.Position.Y + rock.Texture.Height * 1.2f;
            float topGround = _ground.Position.Y * 0.9f;
            coinY = _rand.NextSingle(bottomRock, topGround) + origin;
        }
        else
        {
            // Если камень является нижним, то монета будет расположена между нижним камнем и верхней границей игрового мира
            float topScreen = rock.Position.Y / 2 * 0.6f;
            float topRock = rock.Position.Y * 0.8f;
            coinY = _rand.NextSingle(topScreen, topRock) - origin;
        }

        return new Vector2(coinX, coinY);
    }

    private Rock GetInactiveRock()
    {
        var inactiveRocks = _rocks.Where(r => !r.IsActive).ToList();
        if (_previousRock != null)
        {
            double probability = _rand.NextDouble();
            var oppsiteRocks = inactiveRocks.Where(r => r.RockIsUp != _previousRock.RockIsUp).ToList();
            var identicalRocks = inactiveRocks.Where(r => r.RockIsUp == _previousRock.RockIsUp).ToList();

            if (probability > 0.2)
                return oppsiteRocks[_rand.Next(oppsiteRocks.Count)];
            return identicalRocks[_rand.Next(identicalRocks.Count)];
        }
        return inactiveRocks[_rand.Next(inactiveRocks.Count)];
    }

    private bool IntersectPixels(
                            Matrix transformA, int widthA, int heightA, Color[] dataA,
                            Matrix transformB, int widthB, int heightB, Color[] dataB)
    {

        Matrix transformAToB = transformA * Matrix.Invert(transformB);

        Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
        Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

        Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);


        for (int yA = 0; yA < heightA; yA++)
        {
            Vector2 posInB = yPosInB;

            for (int xA = 0; xA < widthA; xA++)
            {
                int xB = (int)Math.Round(posInB.X);
                int yB = (int)Math.Round(posInB.Y);

                if (0 <= xB && xB < widthB &&
                    0 <= yB && yB < heightB)
                {
                    Color colorA = dataA[xA + yA * widthA];
                    Color colorB = dataB[xB + yB * widthB];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
                posInB += stepX;
            }
            yPosInB += stepY;
        }
        return false;
    }

    #endregion

    #region Draw
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var rock in _rocks)
            rock.Draw(spriteBatch);
        foreach (var coin in _coins)
            coin.Draw(spriteBatch);
        _ground.Draw(spriteBatch);
        _player.Draw(gameTime, spriteBatch);
    }
    #endregion
}
