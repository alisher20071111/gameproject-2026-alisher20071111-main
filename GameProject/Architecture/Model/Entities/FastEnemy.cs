using System;

namespace GameProject.Architecture.Model.Entities
{
    public class FastEnemy : Enemy
    {
        public FastEnemy(float x, float y)
            : base(x, y, 30, 2.5f, 1)
        {
        }

        public override void UpdatePosition(Player player)
        {
            float dx = player.X - X;
            float dy = player.Y - Y;

            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            if (length > 0)
            {
                dx /= length;
                dy /= length;

                X += dx * Speed;
                Y += dy * Speed;
            }
        }
    }
}