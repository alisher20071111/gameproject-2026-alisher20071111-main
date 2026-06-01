namespace GameProject.Architecture.Model.Entities
{
    public abstract class Enemy
    {
        public float X { get; protected set; }
        public float Y { get; protected set; }

        public int Health { get; protected set; }
        public int Damage { get; protected set; }

        public float Speed { get; protected set; }

        public bool IsDead => Health <= 0;

        protected Enemy(float x, float y, int hp, float speed, int damage)
        {
            X = x;
            Y = y;

            Health = hp;
            Speed = speed;
            Damage = damage;
        }

        public abstract void UpdatePosition(Player player);

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (Health < 0)
                Health = 0;
        }
    }
}