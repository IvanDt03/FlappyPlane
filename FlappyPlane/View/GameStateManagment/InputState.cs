using Microsoft.Xna.Framework.Input;

namespace FlappyPlane.View.GameStateManagment;

public class InputState
{

    public KeyboardState CurrentKeyBoardState;
    public KeyboardState LastKeybordState;

    public void Update()
    {
        LastKeybordState = CurrentKeyBoardState;

        CurrentKeyBoardState = Keyboard.GetState();
    }

    public bool IsKeyPrssed(Keys key)
    {
        return CurrentKeyBoardState.IsKeyDown(key);
    }

    public bool IsNewKeyPress(Keys key)
    {
        return CurrentKeyBoardState.IsKeyDown(key) &&
            LastKeybordState.IsKeyUp(key);
    }
}
