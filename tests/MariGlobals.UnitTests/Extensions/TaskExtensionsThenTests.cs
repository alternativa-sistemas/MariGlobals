using MariGlobals.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MariGlobals.UnitTests.Extensions
{
    public class TaskExtensionsThenTests
    {
        [Fact]
        public async Task Then_Should_Throw_ArgumentNull_When_task_Is_Null()
        {
            // Act
            static Task continueFunc() => Task.CompletedTask;
            Task task = null;

            // Arrange + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return task.Then(continueFunc);
            });
        }

        [Fact]
        public async Task Then_Should_Throw_ArgumentNull_When_continueFunc_Is_Null()
        {
            // Act
            Func<Task> continueFunc = null;
            var task = Task.CompletedTask;

            // Arrange + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return task.Then(continueFunc);
            });
        }

        [Fact]
        public async Task Then_Should_Throw_ArgumentNull_When_continueAction_Is_Null()
        {
            // Act
            Action continueAction = null;
            var task = Task.CompletedTask;

            // Arrange + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return task.Then(continueAction);
            });
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTask_When_task_Completes()
        {
            // Act
            var called = false;
            var task = Task.CompletedTask;

            // Arrange
            await task.Then(() =>
            {
                called = true;
                return Task.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTaskTask_When_task_Completes()
        {
            // Act
            var called = false;
            var task = Task.CompletedTask;

            // Arrange
            await task.Then((task) =>
            {
                if (task.IsCompleted)
                    called = true;

                return Task.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTaskOfTResult_When_task_Completes()
        {
            // Act
            var task = Task.CompletedTask;
            var expected = "test";

            // Arrange
            var result = await task.Then(() =>
            {
                return Task.FromResult(expected);
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTaskTaskOfTResult_When_task_Completes()
        {
            // Act
            var task = Task.CompletedTask;
            var expected = "test";

            // Arrange
            var result = await task.Then((task) =>
            {
                if (task.IsCompleted)
                    return Task.FromResult(expected);

                return Task.FromResult("error");
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTResultTask_When_task_Completes()
        {
            // Act
            var expected = "test";
            var task = Task.FromResult(expected);
            var result = string.Empty;

            // Arrange
            await task.Then((taskResult) =>
            {
                result = taskResult;

                return Task.CompletedTask;
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTSourceResultTaskOfTResult_When_task_Completes()
        {
            // Act
            var expectedTaskResult = "test";
            var task = Task.FromResult(expectedTaskResult);
            var expected = true;

            // Arrange
            var result = await task.Then((taskResult) =>
            {
                if (expectedTaskResult.Equals(taskResult))
                    return Task.FromResult(true);

                return Task.FromResult(false);
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTResult_When_task_Completes()
        {
            // Act
            var expected = "test";
            var task = Task.CompletedTask;

            // Arrange
            var result = await task.Then(() =>
            {
                return expected;
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTaskTResult_When_task_Completes()
        {
            // Act
            var expected = "test";
            var task = Task.CompletedTask;

            // Arrange
            var result = await task.Then((task) =>
            {
                if (task.IsCompleted)
                    return expected;

                return "error";
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTSourceResultTResult_When_task_Completes()
        {
            // Act
            var expectedTaskResult = "test";
            var task = Task.FromResult(expectedTaskResult);
            var expected = true;

            // Arrange
            var result = await task.Then((taskResult) =>
            {
                if (expectedTaskResult.Equals(taskResult))
                    return true;

                return false;
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueAction_When_task_Completes()
        {
            // Act
            var called = false;
            var task = Task.CompletedTask;

            // Arrange
            await task.Then(() =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task Then_Should_Call_continueActionTask_When_task_Completes()
        {
            // Act
            var called = false;
            var task = Task.CompletedTask;

            // Arrange
            await task.Then((task) =>
            {
                if (task.IsCompleted)
                    called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task Then_Should_Call_continueActionTResult_When_task_Completes()
        {
            // Act
            var called = false;
            var expectedTaskResult = "test";
            var task = Task.FromResult(expectedTaskResult);

            // Arrange
            await task.Then((result) =>
            {
                if (expectedTaskResult.Equals(result))
                    called = true;
            });

            // Assert
            Assert.True(called);
        }
    }
}