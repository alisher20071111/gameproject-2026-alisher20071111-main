namespace GameProject.Architecture.Model.Entities
{
    public class ExperienceOrb
    {
        public float X;
        public float Y;

        public bool IsCollected;

        public ExperienceOrb(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}