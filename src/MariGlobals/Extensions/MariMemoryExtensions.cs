using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MariGlobals.Extensions
{
    /// <summary>
    /// General extensions for <see cref="Memory{T}" />.
    /// </summary>
    public static class MariMemoryExtensions
    {
        /// <summary>
        /// Try add an object in this <see cref="Memory{T}" />.
        /// </summary>
        /// <param name="memory">The <see cref="Memory{T}" /> that will have
        /// this object added.</param>
        /// <param name="obj">The object to be added.</param>
        /// <returns>True if can add this object.</returns>
        public static bool TryAdd<T>(this Memory<T> memory, T obj)
            => memory.Span.TryAdd(obj);

        /// <summary>
        /// Forcibly adds an object in this <see cref="Memory{T}" /> if can't, creates a new
        /// <see cref="Memory{T}" /> with the double of the length, add all values and 
        /// the new object.
        /// </summary>
        /// <param name="memory">The <see cref="Memory{T}" /> that will have 
        /// this object added.</param>
        /// <param name="obj">The object to be added.</param>
        /// <returns>This <see cref="Memory{T}" /> or a new with the object added.</returns>
        public static Memory<T> ForceAdd<T>(this Memory<T> memory, T obj)
        {
            if (memory.TryAdd(obj))
                return memory;

            var newMemory = new Memory<T>(new T[unchecked(memory.Length * 2)]);

            memory.CopyTo(newMemory);

            newMemory.TryAdd(obj);

            return newMemory;
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}" /> in a <see cref="Memory{T}" />.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to be converted.</param>
        /// <returns>A <see cref="Memory{T}" /> with all values of this 
        /// <see cref="IEnumerable{T}" />.</returns>
        public static Memory<T> ToMemory<T>(this IEnumerable<T> enumerable)
            => new Memory<T>(enumerable.ToArray());

        /// <summary>
        /// Converts an <see cref="ICollection{T}" /> in a <see cref="Memory{T}" />.
        /// </summary>
        /// <param name="enumerable">The <see cref="ICollection{T}" /> to be converted.</param>
        /// <param name="length">The new length of the <see cref="Memory{T}" />.</param>
        /// <returns>A <see cref="Memory{T}" /> with all values of this 
        /// <see cref="ICollection{T}" />.</returns>
        public static Memory<T> ToMemory<T>(this ICollection<T> enumerable, int length)
        {
            if (length < enumerable.Count)
                throw new ArgumentOutOfRangeException(nameof(length));

            var arr = new T[length];
            enumerable.CopyTo(arr, 0);
            return new Memory<T>(arr);
        }

        /// <summary>
        /// Converts a object of <typeparam ref="T" /> to a <see cref="Memory{T}" />.
        /// </summary>
        /// <param name="obj">The object to be converted.</param>
        /// <param name="length">The new length of the <see cref="Memory{T}" />.</param>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>A new <see cref="Memory{T}" /> of <typeparam ref="T" /> with the 
        /// specified length.</returns>
        public static Memory<T> AsMemory<T>(this T obj, int length = 1)
            => new Memory<T>(obj.CreateArray(length));

        /// <summary>
        /// Asynchronously converts the <see cref="Task" /> result to a <see cref="Memory{T}" /> 
        /// of <typeparam ref="T" />.
        /// </summary>
        /// <param name="task">A <see cref="Task" /> with the to be converted.</param>
        /// <param name="length">The new length of the <see cref="Memory{T}" />.</param>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>A <see cref="Task" /> with a new <see cref="Memory{T}" /> 
        /// of <typeparam ref="T" /> with the specified length.</returns>
        public static async Task<Memory<T>> AsMemoryAsync<T>(this Task<T> task, int length = 1)
            => new Memory<T>(await task.CreateArrayAsync(length).ConfigureAwait(false));

        /// <summary>
        /// Asynchronously converts the <see cref="ValueTask" /> result to a <see cref="Memory{T}" /> 
        /// of <typeparam ref="T" />.
        /// </summary>
        /// <param name="task">A <see cref="ValueTask" /> with the to be converted.</param>
        /// <param name="length">The new length of the <see cref="Memory{T}" />.</param>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>A <see cref="ValueTask" /> with a new <see cref="Memory{T}" /> 
        /// of <typeparam ref="T" /> with the specified length.</returns>
        public static async ValueTask<Memory<T>> AsMemoryAsync<T>(this ValueTask<T> task, int length = 1)
            => new Memory<T>(await task.CreateArrayAsync(length).ConfigureAwait(false));

        /// <summary>
        /// Converts the current <see cref="IEnumerable{T}" /> in a <see cref="ReadOnlyMemory{T}" />.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to be converted.</param>
        /// <returns>A <see cref="ReadOnlyMemory{T}" /> with all values of this 
        /// <see cref="IEnumerable{T}" />.</returns>
        public static ReadOnlyMemory<T> ToReadOnlyMemory<T>(this IEnumerable<T> enumerable)
            => enumerable.ToMemory();

        /// <summary>
        /// Converts an <see cref="ICollection{T}" /> in a <see cref="ReadOnlyMemory{T}" />.
        /// </summary>
        /// <param name="enumerable">The <see cref="ICollection{T}" /> to be converted.</param>
        /// <param name="length">The new length of the <see cref="ReadOnlyMemory{T}" />.</param>
        /// <returns>A <see cref="ReadOnlyMemory{T}" /> with all values of this 
        /// <see cref="ICollection{T}" />.</returns>
        public static ReadOnlyMemory<T> ToReadOnlyMemory<T>(this ICollection<T> enumerable, int length)
            => enumerable.ToMemory(length);

        /// <summary>
        /// Converts a object of <typeparam ref="T" /> to a <see cref="ReadOnlyMemory{T}" />.
        /// </summary>
        /// <param name="obj">The object to be converted.</param>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>A new <see cref="ReadOnlyMemory{T}" /> of <typeparam ref="T" /> with the 
        /// specified length.</returns>
        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T obj)
            => obj.AsMemory();

        /// <summary>
        /// Asynchronously converts the <see cref="Task" /> result to a <see cref="ReadOnlyMemory{T}" /> 
        /// of <typeparam ref="T" />.
        /// </summary>
        /// <param name="task">A <see cref="Task" /> with the to be converted.</param>
        /// <param name="length">The new length of the <see cref="ReadOnlyMemory{T}" />.</param>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>A <see cref="Task" /> with a new <see cref="ReadOnlyMemory{T}" /> 
        /// of <typeparam ref="T" /> with the specified length.</returns>
        public static async Task<ReadOnlyMemory<T>> AsReadOnlyMemoryAsync<T>(this Task<T> task, int length = 1)
            => await task.AsMemoryAsync(length).ConfigureAwait(false);

        /// <summary>
        /// Asynchronously converts the <see cref="ValueTask" /> result to a <see cref="ReadOnlyMemory{T}" /> 
        /// of <typeparam ref="T" />.
        /// </summary>
        /// <param name="task">A <see cref="ValueTask" /> with the to be converted.</param>
        /// <param name="length">The new length of the <see cref="ReadOnlyMemory{T}" />.</param>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>A <see cref="ValueTask" /> with a new <see cref="ReadOnlyMemory{T}" /> 
        /// of <typeparam ref="T" /> with the specified length.</returns>
        public static async ValueTask<ReadOnlyMemory<T>> ReadOnlyMemoryAsync<T>(this ValueTask<T> task, int length = 1)
            => await task.AsMemoryAsync(length).ConfigureAwait(false);

        /// <summary>
        /// Verify if this <see cref="Memory{T}" /> is not null or empty.
        /// </summary>
        /// <param name="memory">Any <see cref="Memory{T}" />.</param>
        /// <returns>True if this <see cref="Memory{T}" /> is not null or empty.</returns>  
        public static bool HasContent<T>(this Memory<T> memory)
            => !memory.Equals(null) && !memory.IsEmpty && memory.Span[0].HasContent();

        /// <summary>
        /// Verify if this <see cref="Memory{T}" /> is null or empty.
        /// </summary>
        /// <param name="memory">Any <see cref="Memory{T}" />.</param>
        /// <returns>True if this <see cref="Memory{T}" /> is null or empty.</returns>  
        public static bool HasNoContent<T>(this Memory<T> memory)
            => !memory.HasContent();

        /// <summary>
        /// Verify if this <see cref="ReadOnlyMemory{T}" /> is not null or empty.
        /// </summary>
        /// <param name="memory">Any <see cref="ReadOnlyMemory{T}" />.</param>
        /// <returns>True if this <see cref="ReadOnlyMemory{T}" /> is not null or empty.</returns>  
        public static bool HasContent<T>(this ReadOnlyMemory<T> memory)
            => !memory.Equals(null) && !memory.IsEmpty && memory.Span[0].HasContent();

        /// <summary>
        /// Verify if this <see cref="ReadOnlyMemory{T}" /> is null or empty.
        /// </summary>
        /// <param name="memory">Any <see cref="ReadOnlyMemory{T}" />.</param>
        /// <returns>True if this <see cref="ReadOnlyMemory{T}" /> is null or empty.</returns>  
        public static bool HasNoContent<T>(this ReadOnlyMemory<T> memory)
            => !memory.HasContent();

        /// <summary>
        /// Gets the first value of this <see cref="Memory{T}" /> if can't 
        /// return <see langword="default" />.
        /// </summary>
        /// <param name="memory">Any <see cref="Memory{T}" />.</param>
        /// <returns>The first value of this <see cref="Memory{T}" /> or 
        /// <see langword="default" /> if doesn't has nothing.</returns>
        public static T FirstOrDefault<T>(this Memory<T> memory)
            => memory.HasContent() ? memory.Span[0] : default;

        /// <summary>
        /// Gets the first value of this <see cref="ReadOnlyMemory{T}" /> if can't 
        /// return <see langword="default" />.
        /// </summary>
        /// <param name="memory">Any <see cref="ReadOnlyMemory{T}" />.</param>
        /// <returns>The first value of this <see cref="ReadOnlyMemory{T}" /> or 
        /// <see langword="default" /> if doesn't has nothing.</returns>
        public static T FirstOrDefault<T>(this ReadOnlyMemory<T> memory)
            => memory.HasContent() ? memory.Span[0] : default;

        /// <summary>
        /// Gets the last value of this <see cref="Memory{T}" /> if can't 
        /// return <see langword="default" />.
        /// </summary>
        /// <param name="memory">Any <see cref="Memory{T}" />.</param>
        /// <returns>The last value of this <see cref="Memory{T}" /> or 
        /// <see langword="default" /> if doesn't has nothing.</returns>
        public static T LastOrDefault<T>(this Memory<T> memory)
            => memory.HasContent() ? memory.Span[memory.Length - 1] : default;

        /// <summary>
        /// Gets the last value of this <see cref="ReadOnlyMemory{T}" /> if can't 
        /// return <see langword="default" />.
        /// </summary>
        /// <param name="memory">Any <see cref="ReadOnlyMemory{T}" />.</param>
        /// <returns>The last value of this <see cref="ReadOnlyMemory{T}" /> or 
        /// <see langword="default" /> if doesn't has nothing.</returns>
        public static T LastOrDefault<T>(this ReadOnlyMemory<T> memory)
            => memory.HasContent() ? memory.Span[memory.Length - 1] : default;

        /// <summary>
        /// Creates a <see cref="Memory{T}" /> of <typeparam ref="T" /> with the specified length.
        /// </summary>
        /// <param name="length">The max length of the <see cref="Memory{T}" /> to be created.</param>
        /// <typeparam name="T">The type of the new <see cref="Memory{T}" />.</typeparam>
        /// <returns>A new <see cref="Memory{T}" /> with hte specified length.</returns>
        public static Memory<T> CreateMemory<T>(int length = 4)
            => new Memory<T>(new T[length]);
    }
}