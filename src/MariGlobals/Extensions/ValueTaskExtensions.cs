using System;
using System.Threading.Tasks;

namespace MariGlobals.Extensions
{
    /// <summary>
    /// General extensions for <see cref="ValueTask" />.
    /// </summary>
    public static class ValueTaskExtensions
    {
        /// <summary>
        /// Try continues the current <see cref="ValueTask" /> if a exception is throwed invokes
        /// an exception handler.
        /// </summary>
        /// <param name="task">The <see cref="ValueTask" /> to be continued.</param>
        /// <param name="exceptionHandler">A exception handler if the current <see cref="ValueTask" />
        /// throws an exception.</param>
        public static async ValueTask TryAsync(this ValueTask task, Func<Exception, ValueTask> exceptionHandler)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await exceptionHandler(ex).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Try continues the current <see cref="ValueTask" /> if a exception is throwed invokes
        /// an exception handler.
        /// </summary>
        /// <param name="task">The <see cref="ValueTask" /> to be continued.</param>
        /// <param name="exceptionHandler">A exception handler if the current <see cref="ValueTask" />
        /// throws an exception.</param>
        /// <typeparam name="TResult">The type of the result of the current <see cref="ValueTask" />.</typeparam>
        /// <returns>The result of the task or <see langword="default" /> if exception is throw.</returns>
        public static async ValueTask<TResult> TryAsync<TResult>(this ValueTask<TResult> task, Func<Exception, ValueTask> exceptionHandler)
        {
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await exceptionHandler(ex).ConfigureAwait(false);
            }

            return default;
        }

        /// <summary>
        /// Asynchronously creates an array of <typeparamref name="TResult"/>,
        /// await the current <see cref="ValueTask" /> and add the <typeparamref name="TResult"/>
        /// in the array.
        /// </summary>
        /// <param name="task">The current <see cref="ValueTask" />.</param>
        /// <param name="length">The max length of the new array.</param>
        /// <typeparam name="TResult">The result type of the current <see cref="ValueTask" /> and
        /// the type of the new array.</typeparam>
        /// <returns>A <see cref="ValueTask" /> with the new array with the <typeparamref name="TResult"/>
        /// inside this array.</returns>
        public static async ValueTask<TResult[]> CreateArrayAsync<TResult>(this ValueTask<TResult> task, int length = 1)
            => new TResult[length].TryAdd(await task.ConfigureAwait(false));
    }
}