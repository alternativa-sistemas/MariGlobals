using MariGlobals.Utils;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MariGlobals.Extensions
{
    public static partial class ValueTaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckParametersContinueFunc(object continueFunc)
        {
            continueFunc.NotNull(nameof(continueFunc));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckParametersContinueAction(object continueAction)
        {
            continueAction.NotNull(nameof(continueAction));
        }

        /// <summary>
        /// Do a continuation value task when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async ValueTask Then(this ValueTask valueTask, Func<ValueTask> continueFunc)
        {
            CheckParametersContinueFunc(continueFunc);

            await valueTask.ConfigureAwait(false);
            await continueFunc().ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation value task when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async ValueTask Then(this ValueTask valueTask, Func<ValueTask, ValueTask> continueFunc)
        {
            CheckParametersContinueFunc(continueFunc);

            await valueTask.ConfigureAwait(false);
            await continueFunc(valueTask).ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation with custom result value task when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async ValueTask<TResult?> Then<TResult>(this ValueTask valueTask, Func<ValueTask<TResult?>> continueFunc)
        {
            CheckParametersContinueFunc(continueFunc);

            await valueTask.ConfigureAwait(false);
            return await continueFunc().ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation with custom result value task when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async ValueTask<TResult?> Then<TResult>(this ValueTask valueTask, Func<ValueTask, ValueTask<TResult?>> continueFunc)
        {
            CheckParametersContinueFunc(continueFunc);

            await valueTask.ConfigureAwait(false);
            return await continueFunc(valueTask).ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation value task when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async ValueTask Then<TResult>(this ValueTask<TResult?> valueTask, Func<TResult?, ValueTask> continueFunc)
        {
            CheckParametersContinueFunc(continueFunc);

            var result = await valueTask.ConfigureAwait(false);
            await continueFunc(result).ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation with custom result value task when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <typeparam name="TSourceResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async ValueTask<TResult?> Then<TSourceResult, TResult>(this ValueTask<TSourceResult?> valueTask, Func<TSourceResult?, ValueTask<TResult?>> continueFunc)
        {
            CheckParametersContinueFunc(continueFunc);

            var result = await valueTask.ConfigureAwait(false);
            return await continueFunc(result).ConfigureAwait(false);
        }
        /// <summary>
        /// Do a continuation with custom result value task when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async ValueTask<TResult?> Then<TResult>(this ValueTask valueTask, Func<TResult?> continueFunc)
        {
            CheckParametersContinueFunc(continueFunc);

            await valueTask.ConfigureAwait(false);
            return continueFunc();
        }

        /// <summary>
        /// Do a continuation with custom result value task when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async ValueTask<TResult?> Then<TResult>(this ValueTask valueTask, Func<ValueTask, TResult?> continueFunc)
        {
            CheckParametersContinueFunc(continueFunc);

            await valueTask.ConfigureAwait(false);
            return continueFunc(valueTask);
        }

        /// <summary>
        /// Do a continuation with custom result value task when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <typeparam name="TSourceResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async ValueTask<TResult?> Then<TSourceResult, TResult>(this ValueTask<TSourceResult?> valueTask, Func<TSourceResult?, TResult?> continueFunc)
        {
            CheckParametersContinueFunc(continueFunc);

            var result = await valueTask.ConfigureAwait(false);
            return continueFunc(result);
        }

        /// <summary>
        /// Do a continuation action when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueAction">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueAction" /> is null.</exception>
        public static async ValueTask Then(this ValueTask valueTask, Action continueAction)
        {
            CheckParametersContinueAction(continueAction);

            await valueTask.ConfigureAwait(false);
            continueAction();
        }

        /// <summary>
        /// Do a continuation action when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueAction">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueAction" /> is null.</exception>
        public static async ValueTask Then(this ValueTask valueTask, Action<ValueTask> continueAction)
        {
            CheckParametersContinueAction(continueAction);

            await valueTask.ConfigureAwait(false);
            continueAction(valueTask);
        }

        /// <summary>
        /// Do a continuation action when this <paramref name="valueTask" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="valueTask" /> result.</typeparam>
        /// <param name="valueTask">The current task to wait.</param>
        /// <param name="continueAction">The continuation of this task.</param>
        /// <returns>A <see cref="ValueTask"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="valueTask" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueAction" /> is null.</exception>
        public static async ValueTask Then<TResult>(this ValueTask<TResult?> valueTask, Action<TResult?> continueAction)
        {
            CheckParametersContinueAction(continueAction);

            var result = await valueTask.ConfigureAwait(false);
            continueAction(result);
        }
    }
}