using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Architecture.Controller;
using GameProject.Architecture.Model;
using GameProject.Architecture.View;

namespace GameProject
{
    public class GameEngine : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameModel gameModel;
        private GameController gameController;

        private GameView gameView;

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            gameModel = new GameModel(1280, 720);
            gameController = new GameController(gameModel);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            gameView = new GameView(GraphicsDevice, Content, gameModel);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gameController.ProcessInput(gameTime);
            gameModel.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(15, 15, 20));

            spriteBatch.Begin();

            gameView.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}