using System;
using System.Collections.Generic;
using System.Linq;

namespace MariGlobals.Utils
{
    public static class MariSpanExtensions
    {
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

        public static Span<T> ForceAdd<T>(this Span<T> span, T obj)
        {
            if (span.TryAdd(obj))
                return span;
            else
                return new Span<T>(new T[span.Length * 2].TryAddMany(span.ToArray()).TryAdd(obj));
        }

        public static Span<T> ToSpan<T>(this IEnumerable<T> enumerable)
            => new Span<T>(enumerable.ToArray());

        public static Span<T> ToSpan<T>(this ICollection<T> enumerable, int length)
        {
            if (length < enumerable.Count)
                throw new ArgumentOutOfRangeException(nameof(length));

            var arr = new T[length];
            enumerable.CopyTo(arr, 0);
            return new Span<T>(arr);
        }

        public static Span<T> AsSpan<T>(this T obj, int length = 1)
            => new Span<T>(obj.CreateArray(length));

        public static ReadOnlySpan<T> ToReadOnlySpan<T>(this IEnumerable<T> enumerable)
            => enumerable.ToSpan();

        public static ReadOnlySpan<T> ToReadOnlySpan<T>(this ICollection<T> enumerable, int length)
            => enumerable.ToSpan(length);

        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T obj, int length = 1)
            => obj.AsSpan(length);

        public static bool HasContent<T>(this Span<T> span)
            => span != null && !span.IsEmpty && span[0].HasContent();

        public static bool HasNoContent<T>(this Span<T> span)
            => !span.HasContent();

        public static bool HasContent<T>(this ReadOnlySpan<T> span)
            => span != null && !span.IsEmpty && span[0].HasContent();

        public static bool HasNoContent<T>(this ReadOnlySpan<T> span)
            => !span.HasContent();

        public static T FirstOrDefault<T>(this Span<T> span)
            => span.HasContent() ? span[0] : default;

        public static T FirstOrDefault<T>(this ReadOnlySpan<T> span)
            => span.HasContent() ? span[0] : default;

        public static T LastOrDefault<T>(this Span<T> span)
            => span.HasContent() ? span[span.Length - 1] : default;

        public static T LastOrDefault<T>(this ReadOnlySpan<T> span)
            => span.HasContent() ? span[span.Length - 1] : default;

        public static Span<T> CreateSpan<T>(int length = 4)
            => new Span<T>(new T[length]);

        public static ReadOnlySpan<T> CreateReadOnlySpan<T>(int length = 4)
            => CreateSpan<T>(length);
    }
}