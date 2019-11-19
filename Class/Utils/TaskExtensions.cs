using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MariGlobals.Class.Utils
{
    public static class TaskExtensions
    {
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

        public static async Task<TResult[]> CreateArrayAsync<TResult>(this Task<TResult> task, int length = 1)
            => new TResult[length].TryAdd(await task);
    }
}