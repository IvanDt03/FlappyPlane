using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FlappyPlane.Model;

// Класс анимации текстур 
public class Animation
{
    public Texture2D Texture => _textures[_indexFrame];
    private Texture2D[] _textures;

    private int _indexFrame;

    public float FrameTime { get; }
    private float _amountTime;

    public bool Pause
    {
        get { return _paused; }
        set { _paused = value; }    
    }
    private bool _paused;

    private void Update(GameTime gameTime)
    {
        if (_paused)
            return;

        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _amountTime += elapsed;
        if (_amountTime > FrameTime)
        {
            _indexFrame = (_indexFrame + 1) % _textures.Length;
            _amountTime -= FrameTime;
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float rotation)
    {
        Update(gameTime);

        spriteBatch.Draw(_textures[_indexFrame], 
            position, 
            null, 
            Color.White, 
            rotation, 
            origin, 
            1.0f, 
            SpriteEffects.None, 
            0.0f);
    }

    
    public Animation(Texture2D[] textures, float frameTime)
    {
        _textures = textures;
        FrameTime = frameTime;
        _paused = false;
    }
}
