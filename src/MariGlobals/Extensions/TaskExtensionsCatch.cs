using MariGlobals.Utils;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace MariGlobals.Extensions
{
    public static partial class TaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckParametersCatchFunc(object task, object catchFunc)
        {
            task.NotNull(nameof(task));
            catchFunc.NotNull(nameof(catchFunc));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckParametersCatchAction(object task, object catchAction)
        {
            task.NotNull(nameof(task));
            catchAction.NotNull(nameof(catchAction));
        }

        /// <summary>
        /// Catch any exception that can throw by this <paramref name="task" />.
        /// </summary>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task Catch(this Task task, Func<ExceptionDispatchInfo, Task> catchFunc)
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                await catchFunc(edi).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be caught.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task CatchWhenIs<TException>(this Task task, Func<ExceptionDispatchInfo, Task> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await task.ConfigureAwait(false);
            }
            catch (TException ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                await catchFunc(edi).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task CatchWhenIsNot<TException>(this Task task, Func<ExceptionDispatchInfo, Task> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not TException)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                await catchFunc(edi).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Catch any exception that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task<TResult?> Catch<TResult>(this Task<TResult?> task, Func<ExceptionDispatchInfo, Task> catchFunc)
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                await catchFunc(edi).ConfigureAwait(false);
            }

            return default;
        }

        /// <summary>
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task<TResult?> CatchWhenIs<TException, TResult>(this Task<TResult?> task, Func<ExceptionDispatchInfo, Task> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (TException ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                await catchFunc(edi).ConfigureAwait(false);
            }

            return default;
        }

        /// <summary>
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task<TResult?> CatchWhenIsNot<TException, TResult>(this Task<TResult?> task, Func<ExceptionDispatchInfo, Task> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not TException)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                await catchFunc(edi).ConfigureAwait(false);
            }

            return default;
        }

        /// <summary>
        /// Catch any exception that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task<TResult?> Catch<TResult>(this Task<TResult?> task, Func<ExceptionDispatchInfo, Task<TResult?>> catchFunc)
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                return await catchFunc(edi).ConfigureAwait(false);
            }

            // should never reach here
            return default;
        }

        /// <summary>
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task<TResult?> CatchWhenIs<TException, TResult>(this Task<TResult?> task, Func<ExceptionDispatchInfo, Task<TResult?>> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (TException ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                return await catchFunc(edi).ConfigureAwait(false);
            }

            // should never reach here
            return default;
        }

        /// <summary>
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task<TResult?> CatchWhenIsNot<TException, TResult>(this Task<TResult?> task, Func<ExceptionDispatchInfo, Task<TResult?>> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not TException)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                return await catchFunc(edi).ConfigureAwait(false);
            }

            // should never reach here
            return default;
        }

        /// <summary>
        /// Catch any exception that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task<TResult?> Catch<TResult>(this Task<TResult?> task, Func<ExceptionDispatchInfo, TResult?> catchFunc)
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                return catchFunc(edi);
            }

            // should never reach here
            return default;
        }

        /// <summary>
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task<TResult?> CatchWhenIs<TException, TResult>(this Task<TResult?> task, Func<ExceptionDispatchInfo, TResult?> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (TException ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                return catchFunc(edi);
            }

            // should never reach here
            return default;
        }

        /// <summary>
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async Task<TResult?> CatchWhenIsNot<TException, TResult>(this Task<TResult?> task, Func<ExceptionDispatchInfo, TResult?> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(task, catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not TException)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                return catchFunc(edi);
            }

            // should never reach here
            return default;
        }

        /// <summary>
        /// Catch any exception that can throw by this <paramref name="task" />.
        /// </summary>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async Task Catch(this Task task, Action<ExceptionDispatchInfo> catchAction)
        {
            CheckParametersCatchAction(task, catchAction);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                catchAction(edi);
            }
        }

        /// <summary>
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be caught.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async Task CatchWhenIs<TException>(this Task task, Action<ExceptionDispatchInfo> catchAction)
            where TException : Exception
        {
            CheckParametersCatchAction(task, catchAction);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await task.ConfigureAwait(false);
            }
            catch (TException ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                catchAction(edi);
            }
        }

        /// <summary>
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async Task CatchWhenIsNot<TException>(this Task task, Action<ExceptionDispatchInfo> catchAction)
            where TException : Exception
        {
            CheckParametersCatchAction(task, catchAction);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not TException)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                catchAction(edi);
            }
        }

        /// <summary>
        /// Catch any exception that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async Task<TResult?> Catch<TResult>(this Task<TResult?> task, Action<ExceptionDispatchInfo> catchAction)
        {
            CheckParametersCatchAction(task, catchAction);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                catchAction(edi);
            }

            return default;
        }

        /// <summary>
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async Task<TResult?> CatchWhenIs<TException, TResult>(this Task<TResult?> task, Action<ExceptionDispatchInfo> catchAction)
            where TException : Exception
        {
            CheckParametersCatchAction(task, catchAction);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (TException ex)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                catchAction(edi);
            }

            return default;
        }

        /// <summary>
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="task" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="Task" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async Task<TResult?> CatchWhenIsNot<TException, TResult>(this Task<TResult?> task, Action<ExceptionDispatchInfo> catchAction)
            where TException : Exception
        {
            CheckParametersCatchAction(task, catchAction);

            ExceptionDispatchInfo? edi;
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is not TException)
            {
                edi = ExceptionDispatchInfo.Capture(ex);
            }

            if (edi != null)
            {
                catchAction(edi);
            }

            return default;
        }
    }
}