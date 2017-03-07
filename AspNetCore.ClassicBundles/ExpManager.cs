using System;
using System.IO;

namespace AspNetCore.ClassicBundles
{
    internal static class ExpManager
    {
        public static void ThrowIfNull(object o)
        {
            if (o == null)
                throw new NullReferenceException();
        }

        public static void ThrowIfFalse(bool condition)
        {
            if (!condition)
                throw new InvalidDataException();
        }

        public static void ThrowIfTrue(bool condition)
        {
            if (!condition)
                throw new InvalidDataException();
        }
    }
}