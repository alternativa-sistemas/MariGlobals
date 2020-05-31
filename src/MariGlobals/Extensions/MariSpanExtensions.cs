using System;
using System.Collections.Generic;
using System.Linq;

namespace MariGlobals.Extensions
{
    /// <summary>
    /// General extensions for <see cref="Span{T}" />.
    /// </summary>
    public static class MariSpanExtensions
    {
        /// <summary>
        /// Try add an object in this <see cref="Span{T}" />.
        /// </summary>
        /// <param name="span">The <see cref="Span{T}" /> that will have
        /// this object added.</param>
        /// <param name="obj">The object to be added.</param>
        /// <returns>True if can add this object.</returns>
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

        /// <summary>
        /// Forcibly adds an object in this <see cref="Span{T}" /> if can't, creates a new
        /// <see cref="Span{T}" /> with the double of the length, add all values and 
        /// the new object.
        /// </summary>
        /// <param name="span">The <see cref="Span{T}" /> that will have 
        /// this object added.</param>
        /// <param name="obj">The object to be added.</param>
        /// <returns>This <see cref="Span{T}" /> or a new with the object added.</returns>
        public static Span<T> ForceAdd<T>(this Span<T> span, T obj)
        {
            if (span.TryAdd(obj))
                return span;

            var newSpan = new Span<T>(new T[unchecked(span.Length * 2)]);

            span.CopyTo(newSpan);

            newSpan.TryAdd(obj);

            return newSpan;
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}" /> in a <see cref="Span{T}" />.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to be converted.</param>
        /// <returns>A <see cref="Span{T}" /> with all values of this 
        /// <see cref="IEnumerable{T}" />.</returns>    
        public static Span<T> ToSpan<T>(this IEnumerable<T> enumerable)
            => new Span<T>(enumerable.ToArray());

        /// <summary>
        /// Converts an <see cref="ICollection{T}" /> in a <see cref="Span{T}" />.
        /// </summary>
        /// <param name="enumerable">The <see cref="ICollection{T}" /> to be converted.</param>
        /// <param name="length">The new length of the <see cref="Span{T}" />.</param>
        /// <returns>A <see cref="Span{T}" /> with all values of this 
        /// <see cref="ICollection{T}" />.</returns>
        public static Span<T> ToSpan<T>(this ICollection<T> enumerable, int length)
        {
            if (length < enumerable.Count)
                throw new ArgumentOutOfRangeException(nameof(length));

            var arr = new T[length];
            enumerable.CopyTo(arr, 0);
            return new Span<T>(arr);
        }

        /// <summary>
        /// Converts a object of <typeparam ref="T" /> to a <see cref="Span{T}" />.
        /// </summary>
        /// <param name="obj">The object to be converted.</param>
        /// <param name="length">The new length of the <see cref="Span{T}" />.</param>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>A new <see cref="Span{T}" /> of <typeparam ref="T" /> with the 
        /// specified length.</returns>
        public static Span<T> AsSpan<T>(this T obj, int length = 1)
            => new Span<T>(obj.CreateArray(length));

        /// <summary>
        /// Converts the current <see cref="IEnumerable{T}" /> in a <see cref="ReadOnlySpan{T}" />.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to be converted.</param>
        /// <returns>A <see cref="ReadOnlySpan{T}" /> with all values of this 
        /// <see cref="IEnumerable{T}" />.</returns>
        public static ReadOnlySpan<T> ToReadOnlySpan<T>(this IEnumerable<T> enumerable)
            => enumerable.ToSpan();

        /// <summary>
        /// Converts an <see cref="ICollection{T}" /> in a <see cref="ReadOnlySpan{T}" />.
        /// </summary>
        /// <param name="enumerable">The <see cref="ICollection{T}" /> to be converted.</param>
        /// <param name="length">The new length of the <see cref="ReadOnlySpan{T}" />.</param>
        /// <returns>A <see cref="ReadOnlySpan{T}" /> with all values of this 
        /// <see cref="ICollection{T}" />.</returns>
        public static ReadOnlySpan<T> ToReadOnlySpan<T>(this ICollection<T> enumerable, int length)
            => enumerable.ToSpan(length);

        /// <summary>
        /// Converts a object of <typeparam ref="T" /> to a <see cref="ReadOnlySpan{T}" />.
        /// </summary>
        /// <param name="obj">The object to be converted.</param>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>A new <see cref="ReadOnlySpan{T}" /> of <typeparam ref="T" /> with the 
        /// specified length.</returns>

        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T obj)
            => obj.AsSpan();

        /// <summary>
        /// Verify if this <see cref="Span{T}" /> is not null or empty.
        /// </summary>
        /// <param name="span">Any <see cref="Span{T}" />.</param>
        /// <returns>True if this <see cref="Span{T}" /> is not null or empty.</returns>  
        public static bool HasContent<T>(this Span<T> span)
            => span != null && !span.IsEmpty && span[0].HasContent();

        /// <summary>
        /// Verify if this <see cref="Span{T}" /> is null or empty.
        /// </summary>
        /// <param name="span">Any <see cref="Span{T}" />.</param>
        /// <returns>True if this <see cref="Span{T}" /> is null or empty.</returns>  
        public static bool HasNoContent<T>(this Span<T> span)
            => !span.HasContent();

        /// <summary>
        /// Verify if this <see cref="ReadOnlySpan{T}" /> is not null or empty.
        /// </summary>
        /// <param name="span">Any <see cref="ReadOnlySpan{T}" />.</param>
        /// <returns>True if this <see cref="ReadOnlySpan{T}" /> is not null or empty.</returns>  
        public static bool HasContent<T>(this ReadOnlySpan<T> span)
            => span != null && !span.IsEmpty && span[0].HasContent();

        /// <summary>
        /// Verify if this <see cref="ReadOnlySpan{T}" /> is null or empty.
        /// </summary>
        /// <param name="span">Any <see cref="ReadOnlySpan{T}" />.</param>
        /// <returns>True if this <see cref="ReadOnlySpan{T}" /> is null or empty.</returns>
        public static bool HasNoContent<T>(this ReadOnlySpan<T> span)
            => !span.HasContent();

        /// <summary>
        /// Gets the first value of this <see cref="Span{T}" /> if can't 
        /// return <see langword="default" />.
        /// </summary>
        /// <param name="span">Any <see cref="Span{T}" />.</param>
        /// <returns>The first value of this <see cref="Span{T}" /> or 
        /// <see langword="default" /> if doesn't has nothing.</returns>
        public static T FirstOrDefault<T>(this Span<T> span)
            => span.HasContent() ? span[0] : default;

        /// <summary>
        /// Gets the first value of this <see cref="ReadOnlySpan{T}" /> if can't 
        /// return <see langword="default" />.
        /// </summary>
        /// <param name="span">Any <see cref="ReadOnlySpan{T}" />.</param>
        /// <returns>The first value of this <see cref="ReadOnlySpan{T}" /> or 
        /// <see langword="default" /> if doesn't has nothing.</returns>
        public static T FirstOrDefault<T>(this ReadOnlySpan<T> span)
            => span.HasContent() ? span[0] : default;

        /// <summary>
        /// Gets the last value of this <see cref="Span{T}" /> if can't 
        /// return <see langword="default" />.
        /// </summary>
        /// <param name="span">Any <see cref="Span{T}" />.</param>
        /// <returns>The last value of this <see cref="Span{T}" /> or 
        /// <see langword="default" /> if doesn't has nothing.</returns>
        public static T LastOrDefault<T>(this Span<T> span)
            => span.HasContent() ? span[span.Length - 1] : default;

        /// <summary>
        /// Gets the last value of this <see cref="ReadOnlySpan{T}" /> if can't 
        /// return <see langword="default" />.
        /// </summary>
        /// <param name="span">Any <see cref="ReadOnlySpan{T}" />.</param>
        /// <returns>The last value of this <see cref="ReadOnlySpan{T}" /> or 
        /// <see langword="default" /> if doesn't has nothing.</returns>
        public static T LastOrDefault<T>(this ReadOnlySpan<T> span)
            => span.HasContent() ? span[span.Length - 1] : default;

        /// <summary>
        /// Creates a <see cref="Span{T}" /> of <typeparam ref="T" /> with the specified length.
        /// </summary>
        /// <param name="length">The max length of the <see cref="Span{T}" /> to be created.</param>
        /// <typeparam name="T">The type of the new <see cref="Span{T}" />.</typeparam>
        /// <returns>A new <see cref="Span{T}" /> with hte specified length.</returns>
        public static Span<T> CreateSpan<T>(int length = 4)
            => new Span<T>(new T[length]);
    }
}