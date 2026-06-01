using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject.Architecture.Model;
using GameProject.Architecture.Model.Entities;

namespace GameProject.Architecture.View
{
    public class GameView
    {
        private readonly GameModel gameModel;

        private readonly Texture2D playerTexture;
        private readonly Texture2D enemyTexture;
        private readonly Texture2D fastEnemyTexture;
        private readonly Texture2D bulletTexture;

        private readonly Texture2D pixel;

        private readonly SpriteFont font;

        public GameView(
            GraphicsDevice graphics,
            ContentManager content,
            GameModel model)
        {
            gameModel = model;

            using var p = System.IO.File.OpenRead("Content/player.png");
            using var e = System.IO.File.OpenRead("Content/enemy.png");
            using var fe = System.IO.File.OpenRead("Content/fast_enemy.png");
            using var b = System.IO.File.OpenRead("Content/bullet.png");

            playerTexture = Texture2D.FromStream(graphics, p);

            enemyTexture = Texture2D.FromStream(graphics, e);

            fastEnemyTexture = Texture2D.FromStream(graphics, fe);

            bulletTexture = Texture2D.FromStream(graphics, b);

            font = content.Load<SpriteFont>("DefaultFont");

            pixel = new Texture2D(graphics, 1, 1);

            pixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHealthPacks(spriteBatch);

            DrawPlayer(spriteBatch);

            DrawEnemies(spriteBatch);

            DrawBullets(spriteBatch);

            DrawHud(spriteBatch);

            if (gameModel.IsGameOver)
                DrawGameOver(spriteBatch);
        }

        private void DrawPlayer(SpriteBatch spriteBatch)
        {
            var mouse = Mouse.GetState();

            float dx = mouse.X - gameModel.Player.X;
            float dy = mouse.Y - gameModel.Player.Y;

            float rotation =
                (float)System.Math.Atan2(dy, dx);

            spriteBatch.Draw(
                playerTexture,
                new Vector2(
                    gameModel.Player.X,
                    gameModel.Player.Y),
                null,
                Color.White,
                rotation,
                new Vector2(
                    playerTexture.Width / 2f,
                    playerTexture.Height / 2f),
                2.5f,
                SpriteEffects.None,
                0f);
        }

        private void DrawEnemies(SpriteBatch spriteBatch)
        {
            foreach (var enemy in gameModel.Enemies)
            {
                Texture2D texture;

                int size;

                if (enemy is TankEnemy)
                {
                    texture = enemyTexture;
                    size = 120;
                }
                else
                {
                    texture = fastEnemyTexture;
                    size = 50;
                }

                spriteBatch.Draw(
                    texture,
                    new Rectangle(
                        (int)enemy.X - size / 2,
                        (int)enemy.Y - size / 2,
                        size,
                        size),
                    Color.White);
            }
        }

        private void DrawBullets(SpriteBatch spriteBatch)
        {
            foreach (var bullet in gameModel.BulletPool)
            {
                if (!bullet.IsActive)
                    continue;

                spriteBatch.Draw(
                    bulletTexture,
                    new Rectangle(
                        (int)bullet.X - 10,
                        (int)bullet.Y - 10,
                        20,
                        20),
                    Color.White);
            }
        }

        private void DrawHealthPacks(SpriteBatch spriteBatch)
        {
            foreach (var pack in gameModel.HealthPacks)
            {
                spriteBatch.Draw(
                    pixel,
                    new Rectangle(
                        (int)pack.X - 10,
                        (int)pack.Y - 10,
                        20,
                        20),
                    Color.Red);

                spriteBatch.Draw(
                    pixel,
                    new Rectangle(
                        (int)pack.X - 3,
                        (int)pack.Y - 8,
                        6,
                        16),
                    Color.White);

                spriteBatch.Draw(
                    pixel,
                    new Rectangle(
                        (int)pack.X - 8,
                        (int)pack.Y - 3,
                        16,
                        6),
                    Color.White);
            }
        }
        private void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                pixel,
                new Rectangle(15, 15, 700, 140),
                Color.Black * 0.6f);

            spriteBatch.DrawString(
                font,
                $"HP: {gameModel.Player.CurrentHealth}",
                new Vector2(30, 20),
                Color.White);

            spriteBatch.Draw(
                pixel,
                new Rectangle(30, 55, 350, 30),
                Color.DarkGray);

            spriteBatch.Draw(
                pixel,
                new Rectangle(
                    30,
                    55,
                    gameModel.Player.CurrentHealth * 3,
                    30),
                Color.Red);

            spriteBatch.DrawString(
                font,
                $"LEVEL: {gameModel.Player.Level}",
                new Vector2(30, 100),
                Color.LimeGreen);

            spriteBatch.DrawString(
                font,
                $"FAST KILLS: {gameModel.FastKills}",
                new Vector2(450, 25),
                Color.MediumPurple);

            spriteBatch.DrawString(
                font,
                $"TANK KILLS: {gameModel.TankKills}",
                new Vector2(450, 60),
                Color.OrangeRed);

            spriteBatch.DrawString(
                font,
                $"WAVE: {gameModel.Wave}",
                new Vector2(450, 95),
                Color.Gold);
        }

        private void DrawGameOver(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                pixel,
                new Rectangle(250, 120, 850, 450),
                Color.Black * 0.95f);

            spriteBatch.Draw(
                pixel,
                new Rectangle(300, 170, 750, 320),
                Color.DarkRed);

            spriteBatch.DrawString(
                font,
                "GAME OVER",
                new Vector2(520, 200),
                Color.White);

            spriteBatch.DrawString(
                font,
                $"FAST ENEMIES KILLED: {gameModel.FastKills}",
                new Vector2(420, 280),
                Color.White);

            spriteBatch.DrawString(
                font,
                $"TANK ENEMIES KILLED: {gameModel.TankKills}",
                new Vector2(420, 330),
                Color.White);

            spriteBatch.DrawString(
                font,
                $"TOTAL KILLS: {gameModel.TotalKills}",
                new Vector2(420, 380),
                Color.Gold);

            spriteBatch.DrawString(
                font,
                "PRESS R TO RESTART",
                new Vector2(470, 450),
                Color.LimeGreen);
        }
    }
}