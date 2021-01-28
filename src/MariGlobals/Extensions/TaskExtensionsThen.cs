using MariGlobals.Utils;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MariGlobals.Extensions
{
    public static partial class TaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckParameters(object task, object continueFunc)
        {
            task.NotNull(nameof(task));
            continueFunc.NotNull(nameof(continueFunc));
        }

        /// <summary>
        /// Do a continuation task when this <paramref name="task" /> completes.
        /// </summary>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async Task Then(this Task task, Func<Task> continueFunc)
        {
            CheckParameters(task, continueFunc);

            await task.ConfigureAwait(false);
            await continueFunc().ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation task when this <paramref name="task" /> completes.
        /// </summary>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async Task Then(this Task task, Func<Task, Task> continueFunc)
        {
            CheckParameters(task, continueFunc);

            await task.ConfigureAwait(false);
            await continueFunc(task).ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation with custom result task when this <paramref name="task" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async Task<TResult> Then<TResult>(this Task task, Func<Task<TResult>> continueFunc)
        {
            CheckParameters(task, continueFunc);


            await task.ConfigureAwait(false);
            return await continueFunc().ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation with custom result task when this <paramref name="task" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async Task<TResult> Then<TResult>(this Task task, Func<Task, Task<TResult>> continueFunc)
        {
            CheckParameters(task, continueFunc);

            await task.ConfigureAwait(false);
            return await continueFunc(task).ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation task when this <paramref name="task" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async Task Then<TResult>(this Task<TResult> task, Func<TResult, Task> continueFunc)
        {
            CheckParameters(task, continueFunc);

            var result = await task.ConfigureAwait(false);
            await continueFunc(result).ConfigureAwait(false);
        }

        /// <summary>
        /// Do a continuation with custom result task when this <paramref name="task" /> completes.
        /// </summary>
        /// <typeparam name="TSourceResult">The <paramref name="task" /> result.</typeparam>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async Task<TResult> Then<TSourceResult, TResult>(this Task<TSourceResult> task, Func<TSourceResult, Task<TResult>> continueFunc)
        {
            CheckParameters(task, continueFunc);

            var result = await task.ConfigureAwait(false);
            return await continueFunc(result).ConfigureAwait(false);
        }
        /// <summary>
        /// Do a continuation with custom result task when this <paramref name="task" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async Task<TResult> Then<TResult>(this Task task, Func<TResult> continueFunc)
        {
            CheckParameters(task, continueFunc);

            await task.ConfigureAwait(false);
            return continueFunc();
        }

        /// <summary>
        /// Do a continuation with custom result task when this <paramref name="task" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async Task<TResult> Then<TResult>(this Task task, Func<Task, TResult> continueFunc)
        {
            CheckParameters(task, continueFunc);

            await task.ConfigureAwait(false);
            return continueFunc(task);
        }

        /// <summary>
        /// Do a continuation with custom result task when this <paramref name="task" /> completes.
        /// </summary>
        /// <typeparam name="TSourceResult">The <paramref name="task" /> result.</typeparam>
        /// <typeparam name="TResult">The custom result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueFunc">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueFunc" /> is null.</exception>
        public static async Task<TResult> Then<TSourceResult, TResult>(this Task<TSourceResult> task, Func<TSourceResult, TResult> continueFunc)
        {
            CheckParameters(task, continueFunc);

            var result = await task.ConfigureAwait(false);
            return continueFunc(result);
        }

        /// <summary>
        /// Do a continuation action when this <paramref name="task" /> completes.
        /// </summary>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueAction">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueAction" /> is null.</exception>
        public static Task Then(this Task task, Action continueAction)
        {
            CheckParameters(task, continueAction);

            return task.ContinueWith((_) => continueAction());
        }

        /// <summary>
        /// Do a continuation action when this <paramref name="task" /> completes.
        /// </summary>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueAction">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueAction" /> is null.</exception>
        public static Task Then(this Task task, Action<Task> continueAction)
        {
            CheckParameters(task, continueAction);

            return task.ContinueWith(continueAction);
        }

        /// <summary>
        /// Do a continuation action when this <paramref name="task" /> completes.
        /// </summary>
        /// <typeparam name="TResult">The <paramref name="task" /> result.</typeparam>
        /// <param name="task">The current task to wait.</param>
        /// <param name="continueAction">The continuation of this task.</param>
        /// <returns>A <see cref="Task"/> that represents an asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task" /> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="continueAction" /> is null.</exception>
        public static async Task Then<TResult>(this Task<TResult> task, Action<TResult> continueAction)
        {
            CheckParameters(task, continueAction);

            var result = await task.ConfigureAwait(false);
            continueAction(result);
        }
    }
}