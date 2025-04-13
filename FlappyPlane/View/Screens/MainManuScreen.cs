using FlappyPlane.View.GameStateManagment;
using Microsoft.Xna.Framework.Media;

namespace FlappyPlane.View.Screens;

public class MainManuScreen : MenuScreen
{
    public MainManuScreen() : base("Flappy Plane")
    {
        MenuEntry entryPlayGame = new MenuEntry("Play Game");
        MenuEntry entryOptions = new MenuEntry("Options");
        MenuEntry entryExit = new MenuEntry("Exit");

        entryPlayGame.Selected += EntryPlayGame_Selected;
        entryOptions.Selected += EntryOptions_Selected;
        entryExit.Selected += EntryExit_Selected;

        MenuEntries.Add(entryPlayGame);
        MenuEntries.Add(entryOptions);
        MenuEntries.Add(entryExit);

        if (MediaPlayer.State == MediaState.Stopped)
            MusicManager.PlaybackSong("MainMenu");
    }

    private void EntryExit_Selected(object sender, System.EventArgs e)
    {
        ScreenManager.Game.Exit();
    }

    protected override void OnCancel()
    {
        ScreenManager.Game.Exit();
    }

    private void EntryOptions_Selected(object sender, System.EventArgs e)
    {
        ExitScreen();
        ScreenManager.AddScreen(new OptionsMenuScreen());
    }

    private void EntryPlayGame_Selected(object sender, System.EventArgs e)
    {
        ExitScreen();
        ScreenManager.AddScreen(new GameplayScreen());
        MediaPlayer.Stop();
    }


}
