using UnityEngine;

namespace Assets.Library
{
    public static class Vector2Extensions
    {
        public static Vector2 To(this Vector2 start, Vector2 end)
        => end - start;
    }
}
