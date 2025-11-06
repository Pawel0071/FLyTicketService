using FluentAssertions;
using FLyTicketService.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Middleware
{
    public class ExceptionHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly ExceptionHandlingMiddleware _middleware;

        public ExceptionHandlingMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
            _nextMock = new Mock<RequestDelegate>();
            _middleware = new ExceptionHandlingMiddleware(_nextMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_WhenNoException_CallsNextDelegate()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _nextMock.Setup(x => x(context)).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _nextMock.Verify(x => x(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            
            _nextMock
                .Setup(x => x(context))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_LogsError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var exception = new Exception("Test exception");
            
            _nextMock
                .Setup(x => x(context))
                .ThrowsAsync(exception);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WhenExceptionThrown_WritesErrorMessage()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            
            _nextMock
                .Setup(x => x(context))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            responseBody.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(responseBody);
            var responseText = await reader.ReadToEndAsync();
            responseText.Should().Contain("An unexpected fault happened");
        }

        [Fact]
        public async Task InvokeAsync_WithDifferentExceptionTypes_HandlesAll()
        {
            // Arrange
            var exceptions = new List<Exception>
            {
                new InvalidOperationException("Invalid operation"),
                new ArgumentNullException("Null argument"),
                new DivideByZeroException("Division by zero"),
                new NullReferenceException("Null reference")
            };

            foreach (var exception in exceptions)
            {
                var context = new DefaultHttpContext();
                context.Response.Body = new MemoryStream();
                
                _nextMock
                    .Setup(x => x(context))
                    .ThrowsAsync(exception);

                // Act
                await _middleware.InvokeAsync(context);

                // Assert
                context.Response.StatusCode.Should().Be(500);
            }
        }
    }
}
