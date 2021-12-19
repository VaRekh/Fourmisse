using UnityEngine.Assertions;

namespace Assets.Library
{
    public struct StrictlyPositiveFloat
    {
        public float Value { get; private set; }

        public StrictlyPositiveFloat(float value)
        {
            Assert.IsTrue(value > 0f);
            Value = value;
        }

        public static implicit operator float(StrictlyPositiveFloat value)
            => value.Value;
    }
}
