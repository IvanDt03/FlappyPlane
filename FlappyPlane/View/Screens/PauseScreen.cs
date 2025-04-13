using FlappyPlane.View.GameStateManagment;


namespace FlappyPlane.View.Screens;

public class PauseScreen : MenuScreen
{
    public PauseScreen() : base("Pause")
    {
        MenuEntry entryResume = new MenuEntry("Resume");
        MenuEntry entryQuit = new MenuEntry("Quit");

        entryResume.Selected += EntryResume_Selected;
        entryQuit.Selected += EntryQuit_Selected;

        MenuEntries.Add(entryResume);
        MenuEntries.Add(entryQuit);
    }

    private void EntryResume_Selected(object sender, System.EventArgs e)
    {
        ExitScreen();
    }

    private void EntryQuit_Selected(object sender, System.EventArgs e)
    {
        foreach (var screen in ScreenManager.GetScreens())
        {
            if (screen is BackgroundSceen)
                continue;
            screen.ExitScreen();
        }
            
        ScreenManager.AddScreen(new MainManuScreen());
    }
}
