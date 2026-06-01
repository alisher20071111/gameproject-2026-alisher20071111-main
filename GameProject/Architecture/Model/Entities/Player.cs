namespace GameProject.Architecture.Model.Entities
{
    public class Player
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }

        public int Damage { get; set; }

        public int Level { get; set; }

        public float Speed { get; set; }

        public float BulletSpeed { get; set; }

        public bool IsDead => CurrentHealth <= 0;

        private readonly float arenaWidth;
        private readonly float arenaHeight;

        private readonly float startX;
        private readonly float startY;

        public Player(
            float x,
            float y,
            int hp,
            float speed,
            float width,
            float height)
        {
            X = x;
            Y = y;

            startX = x;
            startY = y;

            arenaWidth = width;
            arenaHeight = height;

            MaxHealth = hp;
            CurrentHealth = hp;

            Speed = speed;

            Damage = 20;

            BulletSpeed = 12f;

            Level = 1;
        }

        public void Move(float dx, float dy)
        {
            X += dx * Speed;
            Y += dy * Speed;

            X = System.Math.Clamp(X, 32, arenaWidth - 32);
            Y = System.Math.Clamp(Y, 32, arenaHeight - 32);
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        public void Heal(int value)
        {
            CurrentHealth += value;

            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }

        public void Reset()
        {
            X = startX;
            Y = startY;

            MaxHealth = 100;
            CurrentHealth = 100;

            Damage = 20;

            BulletSpeed = 12f;

            Speed = 5f;

            Level = 1;
        }
    }
}