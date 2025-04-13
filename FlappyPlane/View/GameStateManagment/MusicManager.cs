using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace FlappyPlane.View.GameStateManagment;

public class MusicManager : GameComponent
{
    public static MusicManager Instance
    {
        get { return _instance; }
    }
    private static MusicManager _instance = null;

    private Random _rand;

    // Volume music
    public static float MusicVolumeMenu
    {
        get { return _instance._musicVolumeMenu; }
        set
        {
            _instance._musicVolumeMenu = MathHelper.Clamp(value, 0.0f, 1.0f);
            MediaPlayer.Volume = _instance._musicVolumeMenu;
        }
    }
    private float _musicVolumeMenu = 0.5f;

    public static float MusicVolumeEffects
    {
        get { return _instance._musicVolumeEffetcs; }
        set
        {
            _instance._musicVolumeEffetcs = MathHelper.Clamp(value, 0.0f, 1.0f);
        }
    }
    private float _musicVolumeEffetcs = 0.2f;

    // Player sounds
    private SoundEffect _soundCoinColletd;
    private SoundEffect _soundJumpPlayer;
    private SoundEffect _soundPlayerDie;

    // Sounds of neture 
    private Song _songWind;
    private SoundEffect[] _soundsNeture;

    // Sounds UI
    private SoundEffect _soundMoveEntry;
    private SoundEffect _soundSelectMenuEntry;
    private Song _songMainMenu;


    private MusicManager(FlappyPlaneGame game) : base(game) 
    {
        _rand = new Random();

        var content = game.Content;

        _soundCoinColletd = content.Load<SoundEffect>("Sounds/CoinCollected");
        _soundPlayerDie = content.Load<SoundEffect>("Sounds/PlayerDie");
        _soundJumpPlayer = content.Load<SoundEffect>("Sounds/PlayerJump");


        _soundMoveEntry = content.Load<SoundEffect>("Sounds/MoveMenuEntry");
        _soundSelectMenuEntry = content.Load<SoundEffect>("Sounds/ClickMenuEntry1");
        _songMainMenu = content.Load<Song>("Sounds/MainMenuMusic");

        _soundsNeture = new SoundEffect[5];
        _soundsNeture[0] = content.Load<SoundEffect>("Sounds/Bird1");
        _soundsNeture[1] = content.Load<SoundEffect>("Sounds/Bird2");
        _soundsNeture[2] = content.Load<SoundEffect>("Sounds/Bird3");
        _soundsNeture[3] = content.Load<SoundEffect>("Sounds/Bird4");
        _soundsNeture[4] = content.Load<SoundEffect>("Sounds/Bird5");
        _songWind = content.Load<Song>("Sounds/Wind");
    }

    public static void Initialize(FlappyPlaneGame game)
    {
        _instance = new MusicManager(game);
        if (game != null)
            game.Components.Add(_instance);
    }



    public static void PlaybackSoundEffetc(string soundName)
    {
        if (_instance == null)
            return;

        switch (soundName)
        {
            case "CoinCollected":
                _instance._soundCoinColletd.Play(_instance._musicVolumeEffetcs, 0.0f, 0.0f);
                break;
            case "PlayerDie":
                _instance._soundPlayerDie.Play(_instance._musicVolumeEffetcs, 0.0f, 0.0f);
                break;
            case "PlayerJump":
                _instance._soundJumpPlayer.Play(_instance._musicVolumeEffetcs, 0.0f, 0.0f);
                break;
            case "SelectedMenuEntry":
                _instance._soundSelectMenuEntry.Play(_instance._musicVolumeEffetcs, 0.0f, 0.0f);
                break;
            case "MoveMenuEntry":
                _instance._soundMoveEntry.Play(_instance._musicVolumeEffetcs, 0.0f, 0.0f);
                break;
            case "Bird":
                int randNeture = _instance._rand.Next(0, _instance._soundsNeture.Length);
                _instance._soundsNeture[randNeture].Play(_instance._musicVolumeEffetcs, 0.0f, 0.0f);
                break;
            
        }
    }

    public static void PlaybackSong(string songName)
    {
        if (_instance == null)
            return;

        switch (songName)
        {
            case "Wind":
                MediaPlayer.Volume = _instance._musicVolumeEffetcs;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(_instance._songWind);
                break;
            case "MainMenu":
                MediaPlayer.Volume = _instance._musicVolumeMenu;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(_instance._songMainMenu);
                break;
        }
    }
}
