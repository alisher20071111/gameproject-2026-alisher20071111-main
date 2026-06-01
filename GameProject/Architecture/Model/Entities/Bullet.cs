namespace GameProject.Architecture.Model.Entities
{
    public class Bullet
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public float DirectionX { get; private set; }
        public float DirectionY { get; private set; }

        public float Speed { get; private set; }

        public int Damage { get; private set; }

        public bool IsActive { get; private set; }

        public void Spawn(
            float x,
            float y,
            float dirX,
            float dirY,
            float speed,
            int damage)
        {
            X = x;
            Y = y;

            DirectionX = dirX;
            DirectionY = dirY;

            Speed = speed;
            Damage = damage;

            IsActive = true;
        }

        public void UpdatePosition()
        {
            X += DirectionX * Speed;
            Y += DirectionY * Speed;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}