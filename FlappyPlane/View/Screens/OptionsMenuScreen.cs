using FlappyPlane.View.GameStateManagment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.DirectoryServices.ActiveDirectory;

namespace FlappyPlane.View.Screens;

public class OptionsMenuScreen : MenuScreen
{

    private MenuEntry _entryTheme;
    private MenuEntry _entryGeneralVolume;
    private MenuEntry _entryEffectsVolume;
    private MenuEntry _entryColorPlane;
    private MenuEntry _entryWorld;

    private InputAction _actionLeft;
    private InputAction _actionRight;

    private static string[] _planeColors = new string[] { "Red", "Yellow", "Blue", "Green" };
    public static string CurrentColorPlane
    {
        get { return _planeColors[_currentColorPlane]; }
    }
    private static int _currentColorPlane = 0;


    private static string[] _worldTypes = new string[] { "Grass", "Ice", "Rock", "Snow", "Dirt" };
    public static string CurrentWorldType
    {
        get { return _worldTypes[_currentTypeWorld]; }
    }
    private static int _currentTypeWorld = 0;

    private static float _currentVolumeEffects = MusicManager.MusicVolumeEffects;
    private static float _currentVolumeMenu = MusicManager.MusicVolumeMenu;

    public OptionsMenuScreen() : base("Options Menu")
    {
        _entryTheme = new MenuEntry(string.Empty);
        _entryColorPlane = new MenuEntry(string.Empty);
        _entryWorld = new MenuEntry(string.Empty);
        _entryGeneralVolume = new VolumeMenuEntry("General volume:", MusicManager.Instance.Game.GraphicsDevice);
        _entryEffectsVolume = new VolumeMenuEntry("Effects volume:", MusicManager.Instance.Game.GraphicsDevice);
        MenuEntry entryBack = new MenuEntry("Back");

        _entryTheme.Selected += EntryTheme_Selected;
        _entryColorPlane.Selected += _entryColorPlane_Selected;
        _entryWorld.Selected += _entryWorld_Selected;
        entryBack.Selected += EntryBack_Selected;

        SetEntryText();

        MenuEntries.Add(_entryGeneralVolume);
        MenuEntries.Add(_entryEffectsVolume);
        MenuEntries.Add(_entryTheme);
        MenuEntries.Add(_entryColorPlane);
        MenuEntries.Add(_entryWorld);
        MenuEntries.Add(entryBack);

        _actionLeft = new InputAction(
            new Keys[] { Keys.A, Keys.Left }, true);
        _actionRight = new InputAction(
            new Keys[] { Keys.D, Keys.Right }, true);
    }

    private void _entryWorld_Selected(object sender, System.EventArgs e)
    {
        _currentTypeWorld = (_currentTypeWorld + 1) % _worldTypes.Length;
        SetEntryText();
    }

    private void _entryColorPlane_Selected(object sender, System.EventArgs e)
    {
        _currentColorPlane = (_currentColorPlane + 1) % _planeColors.Length;
        SetEntryText();
    }

    private void SetEntryText()
    {
        _entryTheme.Text = $"Theme: {ColorPalette.Theme}";
        _entryColorPlane.Text = $"Color plane: {_planeColors[_currentColorPlane]}";
        _entryWorld.Text = $"World: {_worldTypes[_currentTypeWorld]}";
    }

    private void EntryTheme_Selected(object sender, System.EventArgs e)
    {
        ColorPalette.Theme = ColorPalette.Theme == Theme.Light ? Theme.Dark : Theme.Light;
        SetEntryText();
    }

    private void EntryBack_Selected(object sender, System.EventArgs e)
    {
        ExitScreen();
        ScreenManager.AddScreen(new MainManuScreen());
    }

    protected override void OnCancel()
    {
        ExitScreen();
        ScreenManager.AddScreen(new MainManuScreen());
    }

    public override void HandleInput(GameTime gameTime, InputState input)
    {
        base.HandleInput(gameTime, input);

        if (MenuEntries[selectedEntry] == _entryGeneralVolume)
        {
            if (_actionRight.Evaluate(input))
                MusicManager.MusicVolumeMenu += 0.1f;
            else if (_actionLeft.Evaluate(input))
                MusicManager.MusicVolumeMenu -= 0.1f;
        }
        else if (MenuEntries[selectedEntry] == _entryEffectsVolume)
        {
            if (_actionLeft.Evaluate(input))
                MusicManager.MusicVolumeEffects -= 0.1f;
            else if (_actionRight.Evaluate(input))
                MusicManager.MusicVolumeEffects += 0.1f;
        }
    }
}
