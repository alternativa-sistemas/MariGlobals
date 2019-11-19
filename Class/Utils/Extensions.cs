using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static bool OfType<T>(this object obj)
            where T : class
            => (obj as T).HasContent();

        public static bool IsTypeOf<T>(this object obj)
            => obj is T;

        public static async Task TryAsync(this Task task, Func<Exception, Task> exceptionHandler)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                await exceptionHandler(ex);
            }
        }

        public static async Task<TResult> TryAsync<TResult>(this Task<TResult> task, Func<Exception, Task> exceptionHandler)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                await exceptionHandler(ex);
            }

            return default;
        }

        public static async ValueTask TryAsync(this ValueTask task, Func<Exception, Task> exceptionHandler)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                await exceptionHandler(ex);
            }
        }

        public static async ValueTask<TResult> TryAsync<TResult>(this ValueTask<TResult> task, Func<Exception, Task> exceptionHandler)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                await exceptionHandler(ex);
            }

            return default;
        }
    }
}