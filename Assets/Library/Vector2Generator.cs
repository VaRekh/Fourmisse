using UnityEngine;

namespace Assets.Library
{
    public class Vector2Generator
    {
        public Range X { get; }
        public Range Y { get; }

        public Vector2Generator(Range x, Range y)
        {
            X = x;
            Y = y;
        }

        public Vector2 Get()
        {
            float x = Random.Range(X.LowerLimit, X.UpperLimit);
            float y = Random.Range(Y.LowerLimit, Y.UpperLimit);
            Vector2 generated = new Vector2(x, y);
            return generated;
        }

        public Vector2 GetNormalized()
            => Get().normalized;
    }
}
