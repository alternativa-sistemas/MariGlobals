using MariGlobals.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MariGlobals.UnitTests.Extensions
{
    public class ValueTaskExtensionsThenTests
    {
        [Fact]
        public async Task Then_Should_Throw_ArgumentNull_When_continueFunc_Is_Null()
        {
            // Act
            Func<ValueTask> continueFunc = null;
            var valueTask = ValueTask.CompletedTask;

            // Arrange + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await valueTask.Then(continueFunc);
            });
        }

        [Fact]
        public async Task Then_Should_Throw_ArgumentNull_When_continueAction_Is_Null()
        {
            // Act
            Action continueAction = null;
            var valueTask = ValueTask.CompletedTask;

            // Arrange + Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await valueTask.Then(continueAction);
            });
        }

        [Fact]
        public async Task Then_Should_Not_Call_continueAction_When_valueTask_Results_In_Exception()
        {
            // Act
            var valueTask = ValueTask.FromException(new());
            var called = false;

            // Arrange + Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await valueTask.Then(() =>
                {
                    called = true;
                });
            });
            Assert.False(called);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTask_When_valueTask_Completes()
        {
            // Act
            var called = false;
            var valueTask = ValueTask.CompletedTask;

            // Arrange
            await valueTask.Then(() =>
            {
                called = true;
                return ValueTask.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTaskTask_When_valueTask_Completes()
        {
            // Act
            var called = false;
            var valueTask = ValueTask.CompletedTask;

            // Arrange
            await valueTask.Then((task) =>
            {
                if (task.IsCompleted)
                    called = true;

                return ValueTask.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTaskOfTResult_When_valueTask_Completes()
        {
            // Act
            var valueTask = ValueTask.CompletedTask;
            var expected = "test";

            // Arrange
            var result = await valueTask.Then(() =>
            {
                return ValueTask.FromResult(expected);
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTaskTaskOfTResult_When_valueTask_Completes()
        {
            // Act
            var valueTask = ValueTask.CompletedTask;
            var expected = "test";

            // Arrange
            var result = await valueTask.Then((task) =>
            {
                if (task.IsCompleted)
                    return ValueTask.FromResult(expected);

                return ValueTask.FromResult("error");
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTResultTask_When_valueTask_Completes()
        {
            // Act
            var expected = "test";
            var valueTask = ValueTask.FromResult(expected);
            var result = string.Empty;

            // Arrange
            await valueTask.Then((valueTaskResult) =>
            {
                result = valueTaskResult;

                return ValueTask.CompletedTask;
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTSourceResultTaskOfTResult_When_valueTask_Completes()
        {
            // Act
            var expectedValueTaskResult = "test";
            var valueTask = ValueTask.FromResult(expectedValueTaskResult);
            var expected = true;

            // Arrange
            var result = await valueTask.Then((valueTaskResult) =>
            {
                if (expectedValueTaskResult.Equals(valueTaskResult))
                    return ValueTask.FromResult(true);

                return ValueTask.FromResult(false);
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTResult_When_valueTask_Completes()
        {
            // Act
            var expected = "test";
            var valueTask = ValueTask.CompletedTask;

            // Arrange
            var result = await valueTask.Then(() =>
            {
                return expected;
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTaskTResult_When_valueTask_Completes()
        {
            // Act
            var expected = "test";
            var valueTask = ValueTask.CompletedTask;

            // Arrange
            var result = await valueTask.Then((task) =>
            {
                if (task.IsCompleted)
                    return expected;

                return "error";
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueFuncTSourceResultTResult_When_valueTask_Completes()
        {
            // Act
            var expectedValueTaskResult = "test";
            var valueTask = ValueTask.FromResult(expectedValueTaskResult);
            var expected = true;

            // Arrange
            var result = await valueTask.Then((valueTaskResult) =>
            {
                if (expectedValueTaskResult.Equals(valueTaskResult))
                    return true;

                return false;
            });

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Then_Should_Call_continueAction_When_valueTask_Completes()
        {
            // Act
            var called = false;
            var valueTask = ValueTask.CompletedTask;

            // Arrange
            await valueTask.Then(() =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task Then_Should_Call_continueActionTask_When_valueTask_Completes()
        {
            // Act
            var called = false;
            var valueTask = ValueTask.CompletedTask;

            // Arrange
            await valueTask.Then((task) =>
            {
                if (task.IsCompleted)
                    called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task Then_Should_Call_continueActionTResult_When_valueTask_Completes()
        {
            // Act
            var called = false;
            var expectedValueTaskResult = "test";
            var valueTask = ValueTask.FromResult(expectedValueTaskResult);

            // Arrange
            await valueTask.Then((result) =>
            {
                if (expectedValueTaskResult.Equals(result))
                    called = true;
            });

            // Assert
            Assert.True(called);
        }
    }
}