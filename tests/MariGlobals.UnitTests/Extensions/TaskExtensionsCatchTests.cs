using MariGlobals.Extensions;
using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Xunit;

namespace MariGlobals.UnitTests.Extensions
{
    public class TaskExtensionsCatchTests
    {
        [Fact]
        public async Task Catch_Should_Throw_ArgumentNull_When_task_Is_Null()
        {
            // Act
            Task task = null;
            static Task catchFunc(ExceptionDispatchInfo edi) => Task.CompletedTask;

            // Assert + Arrange
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return task.Catch(catchFunc);
            });
        }

        [Fact]
        public async Task Catch_Should_Throw_ArgumentNull_When_catchFunc_Is_Null()
        {
            // Act
            var task = Task.CompletedTask;
            Func<ExceptionDispatchInfo, Task> catchFunc = null;

            // Assert + Arrange
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return task.Catch(catchFunc);
            });
        }

        [Fact]
        public async Task Catch_Should_Not_Call_catchActionEdi_When_task_Completes()
        {
            // Act
            var task = Task.CompletedTask;
            var called = false;

            // Arrange
            await task.Catch((edi) =>
            {
                called = true;
            });

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task Catch_Should_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception()
        {
            // Act
            var exception = new Exception();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            await task.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;

                return Task.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIs_Should_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            await task.CatchWhenIs<ArgumentException>((_) =>
            {
                called = true;

                return Task.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIs_Should_Not_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIs<InvalidOperationException>((_) =>
                {
                    called = true;

                    return Task.CompletedTask;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNot_Should_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            await task.CatchWhenIsNot<InvalidOperationException>((_) =>
            {
                called = true;

                return Task.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNot_Should_Not_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIsNot<InvalidOperationException>((_) =>
                {
                    called = true;

                    return Task.CompletedTask;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchTResult_Should_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception()
        {
            // Act
            var exception = new Exception();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            await task.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;

                return Task.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            await task.CatchWhenIs<ArgumentException>((_) =>
            {
                called = true;

                return Task.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Not_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIs<InvalidOperationException>((_) =>
                {
                    called = true;

                    return Task.CompletedTask;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            await task.CatchWhenIsNot<InvalidOperationException>((_) =>
            {
                called = true;

                return Task.CompletedTask;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Not_Call_catchFuncEdiTask_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIsNot<InvalidOperationException>((_) =>
                {
                    called = true;

                    return Task.CompletedTask;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchTResult_Should_Call_catchFuncEdiTaskOfTTesult_When_Task_Results_In_A_Exception()
        {
            // Act
            var expected = "test";
            var exception = new Exception();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await task.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;

                return Task.FromResult(expected);
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Call_catchFuncEdiTaskOfTTesult_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var expected = "test";
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await task.CatchWhenIs<ArgumentException, string>((_) =>
            {
                called = true;

                return Task.FromResult(expected);
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Not_Call_catchFuncEdiTaskOfTTesult_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIs<InvalidOperationException, string>((_) =>
                {
                    called = true;

                    return Task.FromResult("test");
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Call_catchFuncEdiTaskOfTTesult_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var expected = "test";
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await task.CatchWhenIsNot<InvalidOperationException, string>((_) =>
            {
                called = true;

                return Task.FromResult(expected);
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Not_Call_catchFuncEdiTaskOfTTesult_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIsNot<InvalidOperationException, string>((_) =>
                {
                    called = true;

                    return Task.FromResult("test");
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchTResult_Should_Call_catchFuncEdiTResult_When_Task_Results_In_A_Exception()
        {
            // Act
            var expected = "test";
            var exception = new Exception();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await task.Catch((edi) =>
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
        public async Task CatchWhenIsTResult_Should_Call_catchFuncEdiTResult_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var expected = "test";
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await task.CatchWhenIs<ArgumentException, string>((_) =>
            {
                called = true;

                return expected;
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Not_Call_catchFuncEdiTResult_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIs<InvalidOperationException, string>((_) =>
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
        public async Task CatchWhenIsNotTResult_Should_Call_catchFuncEdiTResult_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var expected = "test";
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            var result = await task.CatchWhenIsNot<InvalidOperationException, string>((_) =>
            {
                called = true;

                return expected;
            });

            // Assert
            Assert.Equal(expected, result);
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Not_Call_catchFuncEdiTResult_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIsNot<InvalidOperationException, string>((_) =>
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
        public async Task Catch_Should_Call_catchActionEdi_When_Task_Results_In_A_Exception()
        {
            // Act
            var exception = new Exception();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            await task.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIs_Should_Call_catchActionEdi_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            await task.CatchWhenIs<ArgumentException>((_) =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIs_Should_Not_Call_catchActionEdi_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIs<InvalidOperationException>((_) =>
                {
                    called = true;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNot_Should_Call_catchActionEdi_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            await task.CatchWhenIsNot<InvalidOperationException>((_) =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNot_Should_Not_Call_catchActionEdi_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var task = Task.FromException(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIsNot<InvalidOperationException>((_) =>
                {
                    called = true;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }


        [Fact]
        public async Task CatchTResult_Should_Call_catchActionEdi_When_Task_Results_In_A_Exception()
        {
            // Act
            var exception = new Exception();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            await task.Catch((edi) =>
            {
                if (edi.SourceException.Equals(exception))
                    called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Call_catchActionEdi_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            await task.CatchWhenIs<ArgumentException, string>((_) =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsTResult_Should_Not_Call_catchActionEdi_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIs<InvalidOperationException, string>((_) =>
                {
                    called = true;
                });
            }
            catch { }

            // Assert
            Assert.False(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Call_catchActionEdi_When_Task_Results_In_A_Exception_And_Exception_Is_Not_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new ArgumentNullException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            await task.CatchWhenIsNot<InvalidOperationException, string>((_) =>
            {
                called = true;
            });

            // Assert
            Assert.True(called);
        }

        [Fact]
        public async Task CatchWhenIsNotTResult_Should_Not_Call_catchActionEdi_When_Task_Results_In_A_Exception_And_Exception_Is_Assigned_From_Specified_Exception_Type()
        {
            // Act
            var exception = new InvalidOperationException();
            var task = Task.FromException<string>(exception);
            var called = false;

            // Arrange
            try
            {
                await task.CatchWhenIsNot<InvalidOperationException, string>((_) =>
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