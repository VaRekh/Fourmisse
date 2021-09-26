namespace Assets.Library
{
    public class Stopwatch
    {

        public float CurrentValue { get; private set; }

        public Stopwatch(float value = 0f)
        {
            CurrentValue = value;
        }


        public void Reset()
            => CurrentValue = 0f;

        public void Update(float deltaTime)
            => CurrentValue += deltaTime;
    }
}
