using System;
using System.Runtime.CompilerServices;

namespace MariGlobals.Utils
{
    internal static class ContentUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NotNull(this object obj, string objName)
        {
            if (obj == null)
                throw new ArgumentNullException(objName);
        }
    }
}