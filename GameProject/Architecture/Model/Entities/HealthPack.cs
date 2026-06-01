namespace GameProject.Architecture.Model.Entities
{
    public class HealthPack
    {
        public float X;
        public float Y;

        public bool IsCollected;

        public HealthPack(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
