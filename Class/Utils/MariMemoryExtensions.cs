using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariGlobals.Class.Utils
{
    public static class MariMemoryExtensions
    {
        public static bool TryAdd<T>(this Memory<T> memory, T obj)
            => memory.Span.TryAdd(obj);

        public static Memory<T> ForceAdd<T>(this Memory<T> memory, T obj)
        {
            if (memory.TryAdd(obj))
                return memory;
            else
                return new Memory<T>(new T[memory.Length * 2].TryAddMany(memory.Span.ToArray()).TryAdd(obj));
        }

        public static Memory<T> AsMemory<T>(this IEnumerable<T> enumerable)
            => new Memory<T>(enumerable.ToArray());

        public static Memory<T> AsMemory<T>(this ICollection<T> enumerable, int length)
        {
            if (length < enumerable.Count)
                throw new ArgumentOutOfRangeException(nameof(length));

            var arr = new T[length];
            enumerable.CopyTo(arr, 0);
            return new Memory<T>(arr);
        }

        public static Memory<T> AsMemory<T>(this T obj, int length = 1)
            => new Memory<T>(obj.CreateArray(length));

        public static async ValueTask<Memory<T>> AsMemoryAsync<T>(this Task<T> task, int length = 1)
            => new Memory<T>(await task.CreateArrayAsync(length).ConfigureAwait(false));

        public static async ValueTask<Memory<T>> AsMemoryAsync<T>(this ValueTask<T> task, int length = 1)
            => new Memory<T>(await task.CreateArrayAsync(length).ConfigureAwait(false));

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this IEnumerable<T> enumerable)
            => enumerable.AsMemory();

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this ICollection<T> enumerable, int length)
            => enumerable.AsMemory(length);

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T obj, int length = 1)
            => obj.AsMemory(length);

        public static async ValueTask<ReadOnlyMemory<T>> AsReadOnlyMemoryAsync<T>(this Task<T> task, int length = 1)
            => await task.AsMemoryAsync(length).ConfigureAwait(false);

        public static async ValueTask<ReadOnlyMemory<T>> ReadOnlyMemoryAsync<T>(this ValueTask<T> task, int length = 1)
            => await task.AsMemoryAsync(length).ConfigureAwait(false);

        public static bool HasContent<T>(this Memory<T> memory)
            => !memory.Equals(null) && !memory.IsEmpty;

        public static bool HasNoContent<T>(this Memory<T> memory)
            => !memory.HasContent();

        public static bool HasContent<T>(this ReadOnlyMemory<T> memory)
            => !memory.Equals(null) && !memory.IsEmpty;

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

        public static ReadOnlyMemory<T> CreateReadOnlyMemory<T>(int length = 4)
            => CreateMemory<T>(length);
    }
}