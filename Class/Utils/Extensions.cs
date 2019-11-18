using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MariGlobals.Class.Utils
{
    public static class Extensions
    {
        public static bool HasContent(this object obj)
            => obj != null;

        public static bool HasNoContent(this object obj)
            => !obj.HasContent();

        public static bool HasContent<T>(this IEnumerable<T> obj)
            => obj != null && obj.Count() > 0;

        public static bool HasNoContent<T>(this IEnumerable<T> obj)
            => !obj.HasContent();

        public static bool TryAdd<T>(this Span<T> span, T obj)
        {
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i].HasContent())
                    continue;

                span[i] = obj;
                return true;
            }

            return false;
        }

        public static bool TryAdd<T>(this Memory<T> memory, T obj)
            => memory.Span.TryAdd(obj);
    }
}