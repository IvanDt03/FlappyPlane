using Microsoft.Xna.Framework.Input;

namespace FlappyPlane.View.GameStateManagment;

public class InputAction
{
    private readonly Keys[] _keys;
    private readonly bool _newPressOnly;

    private delegate bool KeyPress(Keys key);

    public InputAction(Keys[] keys, bool newPressOnly)
    {
        _keys = keys != null ?
            keys.Clone() as Keys[] : new Keys[0];
        _newPressOnly = newPressOnly;
     }

    public bool Evaluate(InputState keyState)
    {
        KeyPress keyTest;

        if (_newPressOnly)
            keyTest = keyState.IsNewKeyPress;
        else
            keyTest = keyState.IsKeyPrssed;


        foreach(var key in _keys)
        {
            if (keyTest(key))
                return true;
        }
        return false;
    }
}
