using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MariGlobals.Extensions
{
    /// <summary>
    /// General programming-life extensions.
    /// </summary>
    public static class MariGlobalsExtensions
    {
        /// <summary>
        /// Verify if this object is not null.
        /// </summary>
        /// <param name="obj">Any <see cref="object" />.</param>
        /// <returns>True if this object is not null.</returns>
        public static bool HasContent(this object obj)
            => obj != null;

        /// <summary>
        /// Verify if this object is null.
        /// </summary>
        /// <param name="obj">Any <see cref="object" />.</param>
        /// <returns>True if this object is null.</returns>
        public static bool HasNoContent(this object obj)
            => !obj.HasContent();

        /// <summary>
        /// Verify if this <see cref="IEnumerable{T}" /> is not null or empty.
        /// </summary>
        /// <param name="obj">Any <see cref="IEnumerable{T}" />.</param>
        /// <returns>True if this <see cref="IEnumerable{T}" /> is not null or empty.</returns>
        public static bool HasContent<T>(this IEnumerable<T> obj)
            => obj != null && obj.Count() > 0;

        /// <summary>
        /// Verify if this <see cref="IEnumerable{T}" /> is null or empty.
        /// </summary>
        /// <param name="obj">Any <see cref="IEnumerable{T}" />.</param>
        /// <returns>True if this <see cref="IEnumerable{T}" /> is null or empty.</returns>        
        public static bool HasNoContent<T>(this IEnumerable<T> obj)
            => !obj.HasContent();

        /// <summary>
        /// Verify if this array is not null or empty.
        /// </summary>
        /// <param name="obj">Any array.</param>
        /// <returns>True if this array is not null or empty.</returns>
        public static bool HasContent<T>(this T[] obj)
            => obj != null && obj.Length > 0;

        /// <summary>
        /// Verify if this array is null or empty.
        /// </summary>
        /// <param name="obj">Any array.</param>
        /// <returns>True if this array is null or empty.</returns>
        public static bool HasNoContent<T>(this T[] obj)
            => !obj.HasContent();

        /// <summary>
        /// Verift if this object is the same type of T, 
        /// using the <see langword="as" /> cast convertion.
        /// </summary>
        /// <param name="obj">Any <see cref="object" /></param>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <returns>True if this <see cref="object" /> is the same time of T.</returns>
        public static bool OfType<T>(this object obj)
            where T : class
            => (obj as T).HasContent();

        /// <summary>
        /// Verift if this object is the same type of T, 
        /// using the <see langword="is" /> cast convertion.
        /// </summary>
        /// <param name="obj">Any <see cref="object" /></param>
        /// <typeparam name="T">The type to be compared.</typeparam>
        /// <returns>True if this <see cref="object" /> is the same time of T.</returns>
        public static bool IsTypeOf<T>(this object obj)
            => obj is T;

        /// <summary>
        /// Create an array of T with the specified length.
        /// </summary>
        /// <param name="obj">Any <see cref="object" />.</param>
        /// <param name="length">The total array's length.</param>
        /// <returns>A new array of T with the specified length.</returns>
        public static T[] CreateArray<T>(this T obj, int length = 1)
            => new T[length].TryAdd(obj);

        /// <summary>
        /// Try add an element in this array, if can't the array will return the same.
        /// </summary>
        /// <param name="array">Any array.</param>
        /// <param name="obj">The object to be added in this array.</param>
        /// <returns>The array with the new element added or not.</returns>
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

        /// <summary>
        /// Try add many entities in this array, 
        /// same of <see cref="MariGlobalsExtensions.TryAdd{T}(T[], T)" /> but
        /// with many entities.
        /// </summary>
        /// <param name="array">Any array.</param>
        /// <param name="objs">The objects to be added in this array.</param>
        /// <returns>he array with the new elements addeds or not.</returns>
        public static T[] TryAddMany<T>(this T[] array, params T[] objs)
        {
            foreach (var obj in objs)
                array = array.TryAdd(obj);

            return array;
        }

        /// <summary>
        /// Converts a <see cref="string" /> to a SHA256.
        /// </summary>
        /// <param name="str">A <see cref="string" /> to be converted to a SHA256.</param>
        /// <returns>A hash representing the SHA256 of the current string.</returns>
        public static string ToSHA256(this string str)
        {
            using var cript = new SHA256Managed();
            var hash = new StringBuilder();
            var bytes = cript.ComputeHash(Encoding.UTF8.GetBytes(str));

            foreach (var computedByte in bytes)
                hash.Append(computedByte.ToString("x2"));

            return hash.ToString();
        }
    }
}