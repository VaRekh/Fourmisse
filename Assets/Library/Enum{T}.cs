using System;

namespace Assets.Library
{
    public static class Enum<T>
        where T : Enum
    {
        public static Array Values
            => Enum.GetValues(typeof(T));

        public static int Count
            => Values.Length;

        public static int Convert(T code)
            => (int)(object)code;

        public static T Convert(int value)
            => (T)(object)value;
    }
}
