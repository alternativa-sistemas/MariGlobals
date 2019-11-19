using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MariGlobals.Class.Utils
{
    public static class ValueTaskExtensions
    {
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

        public static async ValueTask<TResult> TryAsync<TResult>
            (this ValueTask<TResult> task, Func<Exception, ValueTask> exceptionHandler)
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
    }
}