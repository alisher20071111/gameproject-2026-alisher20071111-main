using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using GameProject.Architecture.Model.Entities;

namespace GameProject.Architecture.Model
{
    public class GameModel
    {
        public Player Player { get; }

        public List<Enemy> Enemies { get; }

        public List<Bullet> BulletPool { get; }

        public List<HealthPack> HealthPacks { get; }

        public int FastKills { get; private set; }

        public int TankKills { get; private set; }

        public int TotalKills =>
            FastKills + TankKills;

        public int Wave { get; private set; }

        public bool IsGameOver => Player.IsDead;

        private readonly float arenaWidth;
        private readonly float arenaHeight;

        private readonly Random random;

        public GameModel(float width, float height)
        {
            arenaWidth = width;
            arenaHeight = height;

            random = new Random();

            Player = new Player(
                width / 2,
                height / 2,
                100,
                5f,
                width,
                height);

            Enemies = new List<Enemy>();

            BulletPool = new List<Bullet>();

            HealthPacks = new List<HealthPack>();

            for (int i = 0; i < 200; i++)
                BulletPool.Add(new Bullet());

            Wave = 1;

            SpawnWave();
        }

        public void Update(GameTime gameTime)
        {
            if (Player.IsDead)
                return;

            foreach (var enemy in Enemies)
            {
                enemy.UpdatePosition(Player);

                float dx = enemy.X - Player.X;
                float dy = enemy.Y - Player.Y;

                float distance =
                    (float)Math.Sqrt(dx * dx + dy * dy);

                if (distance < 40f)
                {
                    Player.TakeDamage(enemy.Damage);
                }
            }

            foreach (var pack in HealthPacks)
            {
                float dx = Player.X - pack.X;
                float dy = Player.Y - pack.Y;

                float distance =
                    (float)Math.Sqrt(dx * dx + dy * dy);

                if (distance < 50f)
                {
                    if (Player.CurrentHealth < Player.MaxHealth)
                    {
                        pack.IsCollected = true;

                        Player.Heal(10);
                    }
                }
            }

            HealthPacks.RemoveAll(x => x.IsCollected);

            foreach (var bullet in BulletPool)
            {
                if (!bullet.IsActive)
                    continue;

                bullet.UpdatePosition();

                if (bullet.X < -100 ||
                    bullet.X > arenaWidth + 100 ||
                    bullet.Y < -100 ||
                    bullet.Y > arenaHeight + 100)
                {
                    bullet.Deactivate();
                }
            }

            CheckCollisions();

            Enemies.RemoveAll(e => e.IsDead);

            Player.Level = (TotalKills / 40) + 1;

            if (Enemies.Count == 0)
            {
                Wave++;

                SpawnWave();
            }
        }

        public void FireBullet(float dirX, float dirY)
        {
            foreach (var bullet in BulletPool)
            {
                if (!bullet.IsActive)
                {
                    bullet.Spawn(
                        Player.X,
                        Player.Y,
                        dirX,
                        dirY,
                        Player.BulletSpeed,
                        Player.Damage);

                    break;
                }
            }
        }

        public void Restart()
        {
            Player.Reset();

            Enemies.Clear();

            HealthPacks.Clear();

            foreach (var bullet in BulletPool)
                bullet.Deactivate();

            FastKills = 0;

            TankKills = 0;

            Wave = 1;

            SpawnWave();
        }

        private void SpawnWave()
        {
            int count = Wave * 5;

            for (int i = 0; i < count; i++)
            {
                float x;
                float y;

                int side = random.Next(4);

                switch (side)
                {
                    case 0:
                        x = -50;
                        y = random.Next(0, (int)arenaHeight);
                        break;

                    case 1:
                        x = arenaWidth + 50;
                        y = random.Next(0, (int)arenaHeight);
                        break;

                    case 2:
                        x = random.Next(0, (int)arenaWidth);
                        y = -50;
                        break;

                    default:
                        x = random.Next(0, (int)arenaWidth);
                        y = arenaHeight + 50;
                        break;
                }

                if (random.Next(100) < 50)
                    Enemies.Add(new TankEnemy(x, y));
                else
                    Enemies.Add(new FastEnemy(x, y));
            }
        }

        private void CheckCollisions()
        {
            foreach (var bullet in BulletPool)
            {
                if (!bullet.IsActive)
                    continue;

                foreach (var enemy in Enemies)
                {
                    if (enemy.IsDead)
                        continue;

                    float dx = bullet.X - enemy.X;
                    float dy = bullet.Y - enemy.Y;

                    float distance =
                        (float)Math.Sqrt(dx * dx + dy * dy);

                    if (distance < 30f)
                    {
                        enemy.TakeDamage(bullet.Damage);

                        bullet.Deactivate();

                        if (enemy.IsDead)
                        {
                            if (random.Next(100) < 40)
                            {
                                HealthPacks.Add(
                                    new HealthPack(
                                        enemy.X,
                                        enemy.Y));
                            }

                            if (enemy is FastEnemy)
                                FastKills++;

                            if (enemy is TankEnemy)
                                TankKills++;
                        }

                        break;
                    }
                }
            }
        }
    }
}