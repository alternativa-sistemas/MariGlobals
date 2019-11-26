using System.Collections.Generic;
using System.Linq;

namespace MariGlobals.Utils
{
    public static class MariGlobalsExtensions
    {
        public static bool HasContent(this object obj)
            => obj != null;

        public static bool HasNoContent(this object obj)
            => !obj.HasContent();

        public static bool HasContent<T>(this IEnumerable<T> obj)
            => obj != null && obj.Count() > 0;

        public static bool HasNoContent<T>(this IEnumerable<T> obj)
            => !obj.HasContent();

        public static bool HasContent<T>(this T[] obj)
            => obj != null && obj.Length > 0;

        public static bool HasNoContent<T>(this T[] obj)
            => !obj.HasContent();

        public static bool OfType<T>(this object obj)
            where T : class
            => (obj as T).HasContent();

        public static bool IsTypeOf<T>(this object obj)
            => obj is T;

        public static T[] CreateArray<T>(this T obj, int length = 1)
            => new T[length].TryAdd(obj);

        public static T[] TryAdd<T>(this T[] array, T obj)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].HasContent())
                    continue;

                array[i] = obj;
            }

            return array;
        }

        public static T[] TryAddMany<T>(this T[] array, params T[] objs)
        {
            foreach (var obj in objs)
                array.TryAdd(obj);

            return array;
        }
    }
}