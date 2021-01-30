using MariGlobals.Extensions;
using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xunit;

namespace MariGlobals.UnitTests.Extensions
{
    public class ValueTaskExtensionsCatchTests
    {
        [Fact]
        public async Task Catch_Should_Throw_ArgumentNull_When_catchFunc_Is_Null()
        {
            // Act
            var valueTask = ValueTask.CompletedTask;
            Func<ExceptionDispatchInfo, ValueTask> catchFunc = null;

            // Assert + Arrange
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await valueTask.Catch(catchFunc);
            });
        }

        [Fact]
        public async Task Catch_Should_Not_Call_catchActionEdi_When_valueTask_Completes()
        {
            // Act
            var valueTask = ValueTask.CompletedTask;
            var called = false;

            // Arrange
            await valueTask.Catch((edi) =>
            {
                called = true;
            });

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task Catch_Should_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception()
        {
            // Act
            var exception = new Exception();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            await valueTask.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;

                return ValueTask.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIs_Should_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            await valueTask.CatchWhenIs<ArgumentException>((_) =>
            {
                called = true;

                return ValueTask.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIs_Should_Not_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIs<InvalidOperationException>((_) =>
                {
                    called = true;

                    return ValueTask.CompletedTask;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNot_Should_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            await valueTask.CatchWhenIsNot<InvalidOperationException>((_) =>
            {
                called = true;

                return ValueTask.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNot_Should_Not_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIsNot<InvalidOperationException>((_) =>
                {
                    called = true;

                    return ValueTask.CompletedTask;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchTResult_Should_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception()
        {
            // Act
            var exception = new Exception();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            await valueTask.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;

                return ValueTask.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            await valueTask.CatchWhenIs<ArgumentException, string>((_) =>
            {
                called = true;

                return ValueTask.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Not_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIs<InvalidOperationException, string>((_) =>
                {
                    called = true;

                    return ValueTask.CompletedTask;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            await valueTask.CatchWhenIsNot<InvalidOperationException, string>((_) =>
            {
                called = true;

                return ValueTask.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Not_Call_catchFuncEdiValueTask_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIsNot<InvalidOperationException, string>((_) =>
                {
                    called = true;

                    return ValueTask.CompletedTask;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchTResult_Should_Call_catchFuncEdiValueTaskOfTTesult_When_ValueTask_Results_In_A_Exception()
        {
            // Act
            var expected = "test";
            var exception = new Exception();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await valueTask.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;

                return ValueTask.FromResult(expected);
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Call_catchFuncEdiValueTaskOfTTesult_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var expected = "test";
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await valueTask.CatchWhenIs<ArgumentException, string>((_) =>
            {
                called = true;

                return ValueTask.FromResult(expected);
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Not_Call_catchFuncEdiValueTaskOfTTesult_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIs<InvalidOperationException, string>((_) =>
                {
                    called = true;

                    return ValueTask.FromResult("test");
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Call_catchFuncEdiValueTaskOfTTesult_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var expected = "test";
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await valueTask.CatchWhenIsNot<InvalidOperationException, string>((_) =>
            {
                called = true;

                return ValueTask.FromResult(expected);
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Not_Call_catchFuncEdiValueTaskOfTTesult_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIsNot<InvalidOperationException, string>((_) =>
                {
                    called = true;

                    return ValueTask.FromResult("test");
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchTResult_Should_Call_catchFuncEdiTResult_When_ValueTask_Results_In_A_Exception()
        {
            // Act
            var expected = "test";
            var exception = new Exception();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await valueTask.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;

                return expected;
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Call_catchFuncEdiTResult_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var expected = "test";
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await valueTask.CatchWhenIs<ArgumentException, string>((_) =>
            {
                called = true;

                return expected;
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Not_Call_catchFuncEdiTResult_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIs<InvalidOperationException, string>((_) =>
                {
                    called = true;

                    return "test";
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Call_catchFuncEdiTResult_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var expected = "test";
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await valueTask.CatchWhenIsNot<InvalidOperationException, string>((_) =>
            {
                called = true;

                return expected;
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Not_Call_catchFuncEdiTResult_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIsNot<InvalidOperationException, string>((_) =>
                {
                    called = true;

                    return "test";
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task Catch_Should_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception()
        {
            // Act
            var exception = new Exception();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            await valueTask.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIs_Should_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            await valueTask.CatchWhenIs<ArgumentException>((_) =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIs_Should_Not_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIs<InvalidOperationException>((_) =>
                {
                    called = true;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNot_Should_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            await valueTask.CatchWhenIsNot<InvalidOperationException>((_) =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNot_Should_Not_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var valueTask = ValueTask.FromException(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIsNot<InvalidOperationException>((_) =>
                {
                    called = true;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }


        [Fact]
        public async Task CatchTResult_Should_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception()
        {
            // Act
            var exception = new Exception();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            await valueTask.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            await valueTask.CatchWhenIs<ArgumentException, string>((_) =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Not_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIs<InvalidOperationException, string>((_) =>
                {
                    called = true;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            await valueTask.CatchWhenIsNot<InvalidOperationException, string>((_) =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Not_Call_catchActionEdi_When_ValueTask_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var valueTask = ValueTask.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await valueTask.CatchWhenIsNot<InvalidOperationException, string>((_) =>
                {
                    called = true;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }
    }
}