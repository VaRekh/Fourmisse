namespace Assets.Library
{
    public class Intensity
    {
        public uint Value { get; private set; }

        public Intensity(uint resource_quantity)
        {
            Value = resource_quantity;
        }
    }
}
