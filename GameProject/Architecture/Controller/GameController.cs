using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using GameProject.Architecture.Model;

namespace GameProject.Architecture.Controller
{
    public class GameController
    {
        private readonly GameModel gameModel;

        private float shootCooldown;

        private const float ShootDelay = 0.15f;

        public GameController(GameModel model)
        {
            gameModel = model;
        }

        public void ProcessInput(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (gameModel.IsGameOver)
            {
                if (keyboard.IsKeyDown(Keys.R))
                {
                    gameModel.Restart();
                }

                return;
            }

            float dt =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (shootCooldown > 0)
                shootCooldown -= dt;

            float dx = 0;
            float dy = 0;

            if (keyboard.IsKeyDown(Keys.W))
                dy -= 1;

            if (keyboard.IsKeyDown(Keys.S))
                dy += 1;

            if (keyboard.IsKeyDown(Keys.A))
                dx -= 1;

            if (keyboard.IsKeyDown(Keys.D))
                dx += 1;

            gameModel.Player.Move(dx, dy);

            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed &&
                shootCooldown <= 0)
            {
                float dirX =
                    mouse.X - gameModel.Player.X;

                float dirY =
                    mouse.Y - gameModel.Player.Y;

                float length =
                    (float)Math.Sqrt(dirX * dirX + dirY * dirY);

                if (length > 0)
                {
                    dirX /= length;
                    dirY /= length;

                    gameModel.FireBullet(dirX, dirY);

                    shootCooldown = ShootDelay;
                }
            }
        }
    }
}