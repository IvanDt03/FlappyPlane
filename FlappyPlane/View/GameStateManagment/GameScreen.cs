using Microsoft.Xna.Framework;

namespace FlappyPlane.View.GameStateManagment;

public enum ScreenState
{
    Active, 
    Hidden
}

public abstract class GameScreen
{

    public ScreenState ScreenState
    {
        get { return _screenState; }
    }
    private ScreenState _screenState = ScreenState.Active;

    public bool IsActive
    {
        get
        {
            return !_otherScreenHasFocus &&
                _screenState == ScreenState.Active;
                
        }
    }
    private bool _otherScreenHasFocus;

    public ScreenManager ScreenManager
    {
        get { return _screenManager; }
        internal set { _screenManager = value; }
    }
    private ScreenManager _screenManager;

    public virtual void Load() { }

    public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
    {
        _otherScreenHasFocus = otherScreenHasFocus;

        if (coveredByOtherScreen)
        {
            _screenState = ScreenState.Hidden;
        }
        else
        {
            _screenState = ScreenState.Active;
        }
    }

    public virtual void Draw(GameTime gameTime) { }

    public virtual void HandleInput(GameTime gameTime, InputState input) { }

    public virtual void Unload() { }

    public void ExitScreen()
    {
        ScreenManager.RemoveScreen(this);
    }
}
