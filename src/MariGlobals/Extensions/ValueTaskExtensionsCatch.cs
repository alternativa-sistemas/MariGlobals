using MariGlobals.Utils;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace MariGlobals.Extensions
{
    public static partial class ValueTaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckParametersCatchFunc(object catchFunc)
        {
            catchFunc.NotNull(nameof(catchFunc));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckParametersCatchAction(object catchAction)
        {
            catchAction.NotNull(nameof(catchAction));
        }

        /// <summary>
        /// Catch any exception that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask Catch(this ValueTask valueTask, Func<ExceptionDispatchInfo, ValueTask> catchFunc)
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be caught.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask CatchWhenIs<TException>(this ValueTask valueTask, Func<ExceptionDispatchInfo, ValueTask> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask CatchWhenIsNot<TException>(this ValueTask valueTask, Func<ExceptionDispatchInfo, ValueTask> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await valueTask.ConfigureAwait(false);
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
        /// Catch any exception that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask<TResult?> Catch<TResult>(this ValueTask<TResult?> valueTask, Func<ExceptionDispatchInfo, ValueTask> catchFunc)
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask<TResult?> CatchWhenIs<TException, TResult>(this ValueTask<TResult?> valueTask, Func<ExceptionDispatchInfo, ValueTask> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask<TResult?> CatchWhenIsNot<TException, TResult>(this ValueTask<TResult?> valueTask, Func<ExceptionDispatchInfo, ValueTask> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask<TResult?> Catch<TResult>(this ValueTask<TResult?> valueTask, Func<ExceptionDispatchInfo, ValueTask<TResult?>> catchFunc)
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask<TResult?> CatchWhenIs<TException, TResult>(this ValueTask<TResult?> valueTask, Func<ExceptionDispatchInfo, ValueTask<TResult?>> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask<TResult?> CatchWhenIsNot<TException, TResult>(this ValueTask<TResult?> valueTask, Func<ExceptionDispatchInfo, ValueTask<TResult?>> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask<TResult?> Catch<TResult>(this ValueTask<TResult?> valueTask, Func<ExceptionDispatchInfo, TResult?> catchFunc)
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask<TResult?> CatchWhenIs<TException, TResult>(this ValueTask<TResult?> valueTask, Func<ExceptionDispatchInfo, TResult?> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchFunc">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchFunc" /> is null.</exception>
        public static async ValueTask<TResult?> CatchWhenIsNot<TException, TResult>(this ValueTask<TResult?> valueTask, Func<ExceptionDispatchInfo, TResult?> catchFunc)
            where TException : Exception
        {
            CheckParametersCatchFunc(catchFunc);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async ValueTask Catch(this ValueTask valueTask, Action<ExceptionDispatchInfo> catchAction)
        {
            CheckParametersCatchAction(catchAction);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be caught.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async ValueTask CatchWhenIs<TException>(this ValueTask valueTask, Action<ExceptionDispatchInfo> catchAction)
            where TException : Exception
        {
            CheckParametersCatchAction(catchAction);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async ValueTask CatchWhenIsNot<TException>(this ValueTask valueTask, Action<ExceptionDispatchInfo> catchAction)
            where TException : Exception
        {
            CheckParametersCatchAction(catchAction);

            ExceptionDispatchInfo? edi = null;

            try
            {
                await valueTask.ConfigureAwait(false);
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
        /// Catch any exception that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async ValueTask<TResult?> Catch<TResult>(this ValueTask<TResult?> valueTask, Action<ExceptionDispatchInfo> catchAction)
        {
            CheckParametersCatchAction(catchAction);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is equals or inherits from <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async ValueTask<TResult?> CatchWhenIs<TException, TResult>(this ValueTask<TResult?> valueTask, Action<ExceptionDispatchInfo> catchAction)
            where TException : Exception
        {
            CheckParametersCatchAction(catchAction);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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
        /// Catch any exception when type is not equals <typeparamref name="TException"/> that can throw by this <paramref name="valueTask" />.
        /// </summary>
        /// <typeparam name="TException">The exception type to be not caught.</typeparam>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current valueTask to wait.</param>
        /// <param name="catchAction">The catch exception handler.</param>
        /// <returns>A <see cref="ValueTask" /> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="catchAction" /> is null.</exception>
        public static async ValueTask<TResult?> CatchWhenIsNot<TException, TResult>(this ValueTask<TResult?> valueTask, Action<ExceptionDispatchInfo> catchAction)
            where TException : Exception
        {
            CheckParametersCatchAction(catchAction);

            ExceptionDispatchInfo? edi;
            try
            {
                return await valueTask.ConfigureAwait(false);
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