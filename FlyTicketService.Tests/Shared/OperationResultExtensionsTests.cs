using FluentAssertions;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FlyTicketService.Tests.Shared
{
    public class OperationResultExtensionsTests
    {
        [Theory]
        [InlineData(OperationStatus.Ok, 200)]
        [InlineData(OperationStatus.Created, 201)]
        [InlineData(OperationStatus.NoContent, 204)]
        [InlineData(OperationStatus.BadRequest, 400)]
        [InlineData(OperationStatus.NotFound, 404)]
        [InlineData(OperationStatus.Conflict, 409)]
        [InlineData(OperationStatus.InternalServerError, 500)]
        public void ToInt_WithVariousStatuses_ReturnsCorrectStatusCode(OperationStatus status, int expectedCode)
        {
            // Act
            var result = status.ToInt();

            // Assert
            result.Should().Be(expectedCode);
        }

        [Fact]
        public void GetResult_WithSuccessfulResult_ReturnsObjectResultWithData()
        {
            // Arrange
            var testData = "Test Data";
            var operationResult = new OperationResult<string>(OperationStatus.Ok, null, testData);

            // Act
            var result = operationResult.GetResult();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(200);
            objectResult.Value.Should().Be(testData);
        }

        [Fact]
        public void GetResult_WithCreatedResult_ReturnsObjectResultWith201()
        {
            // Arrange
            var testData = new { Id = 1, Name = "Test" };
            var operationResult = new OperationResult<object>(OperationStatus.Created, null, testData);

            // Act
            var result = operationResult.GetResult();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(201);
            objectResult.Value.Should().Be(testData);
        }

        [Fact]
        public void GetResult_WithNotFoundResult_ReturnsObjectResultWithMessage()
        {
            // Arrange
            var errorMessage = "Resource not found";
            var operationResult = new OperationResult<string>(OperationStatus.NotFound, errorMessage, null);

            // Act
            var result = operationResult.GetResult();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(404);
            objectResult.Value.Should().Be(errorMessage);
        }

        [Fact]
        public void GetResult_WithBadRequestResult_ReturnsObjectResultWithMessage()
        {
            // Arrange
            var errorMessage = "Invalid request";
            var operationResult = new OperationResult<string>(OperationStatus.BadRequest, errorMessage, null);

            // Act
            var result = operationResult.GetResult();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(400);
            objectResult.Value.Should().Be(errorMessage);
        }

        [Fact]
        public void GetResult_WithConflictResult_ReturnsObjectResultWithMessage()
        {
            // Arrange
            var errorMessage = "Resource already exists";
            var operationResult = new OperationResult<string>(OperationStatus.Conflict, errorMessage, null);

            // Act
            var result = operationResult.GetResult();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(409);
            objectResult.Value.Should().Be(errorMessage);
        }

        [Fact]
        public void GetResult_WithInternalServerErrorResult_ReturnsObjectResultWithMessage()
        {
            // Arrange
            var errorMessage = "Internal server error";
            var operationResult = new OperationResult<string>(OperationStatus.InternalServerError, errorMessage, null);

            // Act
            var result = operationResult.GetResult();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(500);
            objectResult.Value.Should().Be(errorMessage);
        }

        [Fact]
        public void GetResult_WithNoContentResult_ReturnsObjectResultWith204()
        {
            // Arrange
            var operationResult = new OperationResult<object>(OperationStatus.NoContent, null, null);

            // Act
            var result = operationResult.GetResult();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(204);
        }

        [Fact]
        public void GetResult_WithComplexObject_ReturnsObjectResultWithCorrectData()
        {
            // Arrange
            var complexData = new
            {
                Id = Guid.NewGuid(),
                Name = "Test Entity",
                Count = 42,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            var operationResult = new OperationResult<object>(OperationStatus.Ok, null, complexData);

            // Act
            var result = operationResult.GetResult();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(200);
            objectResult.Value.Should().BeEquivalentTo(complexData);
        }

        [Fact]
        public void GetResult_WithNullResult_ReturnsObjectResultWithNull()
        {
            // Arrange
            var operationResult = new OperationResult<string?>(OperationStatus.Ok, null, null);

            // Act
            var result = operationResult.GetResult();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult!.StatusCode.Should().Be(200);
            objectResult.Value.Should().BeNull();
        }
    }
}
