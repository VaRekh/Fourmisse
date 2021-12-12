using System;

namespace Assets.Library
{
    public struct StrictlyPositiveFloat
    {
        public float Value { get; private set; }

        public StrictlyPositiveFloat(float value)
        {
            if (value <= 0f)
            {
                throw new ArgumentException();
            }
            Value = value;
        }

        public static implicit operator float(StrictlyPositiveFloat value)
            => value.Value;
    }
}
