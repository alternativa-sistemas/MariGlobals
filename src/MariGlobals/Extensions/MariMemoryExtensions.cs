using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MariGlobals.Extensions
{
    public static class MariMemoryExtensions
    {
        public static bool TryAdd<T>(this Memory<T> memory, T obj)
            => memory.Span.TryAdd(obj);

        public static Memory<T> ForceAdd<T>(this Memory<T> memory, T obj)
        {
            if (memory.TryAdd(obj))
                return memory;

            var newMemory = new Memory<T>(new T[unchecked(memory.Length * 2)]);

            memory.CopyTo(newMemory);

            newMemory.TryAdd(obj);

            return newMemory;
        }

        public static Memory<T> ToMemory<T>(this IEnumerable<T> enumerable)
            => new Memory<T>(enumerable.ToArray());

        public static Memory<T> ToMemory<T>(this ICollection<T> enumerable, int length)
        {
            if (length < enumerable.Count)
                throw new ArgumentOutOfRangeException(nameof(length));

            var arr = new T[length];
            enumerable.CopyTo(arr, 0);
            return new Memory<T>(arr);
        }

        public static Memory<T> AsMemory<T>(this T obj, int length = 1)
            => new Memory<T>(obj.CreateArray(length));

        public static async Task<Memory<T>> AsMemoryAsync<T>(this Task<T> task, int length = 1)
            => new Memory<T>(await task.CreateArrayAsync(length).ConfigureAwait(false));

        public static async ValueTask<Memory<T>> AsMemoryAsync<T>(this ValueTask<T> task, int length = 1)
            => new Memory<T>(await task.CreateArrayAsync(length).ConfigureAwait(false));

        public static ReadOnlyMemory<T> ToReadOnlyMemory<T>(this IEnumerable<T> enumerable)
            => enumerable.ToMemory();

        public static ReadOnlyMemory<T> ToReadOnlyMemory<T>(this ICollection<T> enumerable, int length)
            => enumerable.ToMemory(length);

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T obj, int length = 1)
            => obj.AsMemory(length);

        public static async Task<ReadOnlyMemory<T>> AsReadOnlyMemoryAsync<T>(this Task<T> task, int length = 1)
            => await task.AsMemoryAsync(length).ConfigureAwait(false);

        public static async ValueTask<ReadOnlyMemory<T>> ReadOnlyMemoryAsync<T>(this ValueTask<T> task, int length = 1)
            => await task.AsMemoryAsync(length).ConfigureAwait(false);

        public static bool HasContent<T>(this Memory<T> memory)
            => !memory.Equals(null) && !memory.IsEmpty && memory.Span[0].HasContent();

        public static bool HasNoContent<T>(this Memory<T> memory)
            => !memory.HasContent();

        public static bool HasContent<T>(this ReadOnlyMemory<T> memory)
            => !memory.Equals(null) && !memory.IsEmpty && memory.Span[0].HasContent();

        public static bool HasNoContent<T>(this ReadOnlyMemory<T> memory)
            => !memory.HasContent();

        public static T FirstOrDefault<T>(this Memory<T> memory)
            => memory.HasContent() ? memory.Span[0] : default;

        public static T FirstOrDefault<T>(this ReadOnlyMemory<T> memory)
            => memory.HasContent() ? memory.Span[0] : default;

        public static T LastOrDefault<T>(this Memory<T> memory)
            => memory.HasContent() ? memory.Span[memory.Length - 1] : default;

        public static T LastOrDefault<T>(this ReadOnlyMemory<T> memory)
            => memory.HasContent() ? memory.Span[memory.Length - 1] : default;

        public static Memory<T> CreateMemory<T>(int length = 4)
            => new Memory<T>(new T[length]);
    }
}