namespace Assets.Library
{
    public class Stopwatch
    {

        public float CurrentValue { get; set; }

        public Stopwatch(float value = 0f)
        {
            CurrentValue = value;
        }


        public void Reset(float value = 0f)
            => CurrentValue = value;

        public void Update(float deltaTime)
            => CurrentValue += deltaTime;
    }
}
