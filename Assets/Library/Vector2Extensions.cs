using UnityEngine;

namespace Assets.Library
{
    public static class Vector2Extensions
    {
        public static Vector2 To(this Vector2 start, Vector2 end)
            => end - start;

        public static Vector2 NormalizedDirectionTo(this Vector2 start, Vector2 end)
            => start.To(end).normalized;

        public static float DistanceTo(this Vector2 start, Vector2 end)
            => Vector2.Distance(start, end);

        public static LengthComparison CompareTo(this Vector2 a, Vector2 b)
        {
            LengthComparison result;

            if (a.sqrMagnitude > b.sqrMagnitude)
            {
                result = LengthComparison.LongerThan;
            }
            else if (a.sqrMagnitude == b.sqrMagnitude)
            {
                result = LengthComparison.Equal;
            }
            else
            {
                result = LengthComparison.ShorterThan;
            }

            return result;
        }
    }
}
