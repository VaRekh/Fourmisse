using System;

namespace Assets.Library
{
    public readonly struct Range
    {

        public float LowerLimit { get; }
        public float UpperLimit { get; }


        public Range(float lower_limit, float upper_limit)
        {
            if (lower_limit > upper_limit)
            {
                throw new ArgumentException("Lower Limit should be less than or equal to upper limit");
            }
            LowerLimit = lower_limit;
            UpperLimit = upper_limit;
        }
    }
}
