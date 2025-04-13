using FlappyPlane.View.GameStateManagment;
using FlappyPlane.View.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace FlappyPlane
{
    public class FlappyPlaneGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private ScreenManager _screenManager;
        private MusicManager _musicManager;
        

        public FlappyPlaneGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            _screenManager = new ScreenManager(this);
            MusicManager.Initialize(this);

            Components.Add(_screenManager);
        }

        protected override void Initialize()
        { 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _screenManager.AddScreen(new BackgroundSceen());
            _screenManager.AddScreen(new MainManuScreen());


            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
