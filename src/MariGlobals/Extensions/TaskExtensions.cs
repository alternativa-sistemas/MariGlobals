using System;
using System.Threading.Tasks;

namespace MariGlobals.Extensions
{
    /// <summary>
    /// General extensions for <see cref="Task" />.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Try continues the current <see cref="Task" /> if a exception is throwed invokes
        /// an exception handler.
        /// </summary>
        /// <param name="task">The <see cref="Task" /> to be continued.</param>
        /// <param name="exceptionHandler">A exception handler if the current <see cref="Task" />
        /// throws an exception.</param>
        public static async Task TryAsync(this Task task, Func<Exception, Task> exceptionHandler)
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
        /// Try continues the current <see cref="Task" /> if a exception is throwed invokes
        /// an exception handler.
        /// </summary>
        /// <param name="task">The <see cref="Task" /> to be continued.</param>
        /// <param name="exceptionHandler">A exception handler if the current <see cref="Task" />
        /// throws an exception.</param>
        /// <typeparam name="TResult">The type of the result of the current <see cref="Task" />.</typeparam>
        /// <returns>The result of the task or <see langword="default" /> if exception is throw.</returns>
        public static async Task<TResult> TryAsync<TResult>(this Task<TResult> task, Func<Exception, Task> exceptionHandler)
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
        /// await the current <see cref="Task" /> and add the <typeparamref name="TResult"/>
        /// in the array.
        /// </summary>
        /// <param name="task">The current <see cref="Task" />.</param>
        /// <param name="length">The max length of the new array.</param>
        /// <typeparam name="TResult">The result type of the current <see cref="Task" /> and
        /// the type of the new array.</typeparam>
        /// <returns>A <see cref="Task" /> with the new array with the <typeparamref name="TResult"/>
        /// inside this array.</returns>
        public static async Task<TResult[]> CreateArrayAsync<TResult>(this Task<TResult> task, int length = 1)
            => new TResult[length].TryAdd(await task.ConfigureAwait(false));
    }
}