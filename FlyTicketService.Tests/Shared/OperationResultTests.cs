using FluentAssertions;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FlyTicketService.Tests.Shared
{
    public class OperationResultTests
    {
        [Fact]
        public void OperationResult_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var status = OperationStatus.Ok;
            var message = "Success";
            var data = "Test Data";

            // Act
            var result = new OperationResult<string>(status, message, data);

            // Assert
            result.Status.Should().Be(status);
            result.Message.Should().Be(message);
            result.Result.Should().Be(data);
        }

        [Fact]
        public void IsSuccessStatusCode_WithOkStatus_ReturnsTrue()
        {
            // Arrange
            var result = new OperationResult<string>(OperationStatus.Ok, "Success", "data");

            // Act
            var isSuccess = result.IsSuccessStatusCode();

            // Assert
            isSuccess.Should().BeTrue();
        }

        [Fact]
        public void IsSuccessStatusCode_WithCreatedStatus_ReturnsTrue()
        {
            // Arrange
            var result = new OperationResult<string>(OperationStatus.Created, "Created", "data");

            // Act
            var isSuccess = result.IsSuccessStatusCode();

            // Assert
            isSuccess.Should().BeTrue();
        }

        [Fact]
        public void IsSuccessStatusCode_WithNotFoundStatus_ReturnsFalse()
        {
            // Arrange
            var result = new OperationResult<string>(OperationStatus.NotFound, "Not Found", null);

            // Act
            var isSuccess = result.IsSuccessStatusCode();

            // Assert
            isSuccess.Should().BeFalse();
        }

        [Fact]
        public void IsSuccessStatusCode_WithBadRequestStatus_ReturnsFalse()
        {
            // Arrange
            var result = new OperationResult<string>(OperationStatus.BadRequest, "Bad Request", null);

            // Act
            var isSuccess = result.IsSuccessStatusCode();

            // Assert
            isSuccess.Should().BeFalse();
        }

        [Fact]
        public void IsSuccessStatusCode_WithInternalServerErrorStatus_ReturnsFalse()
        {
            // Arrange
            var result = new OperationResult<string>(OperationStatus.InternalServerError, "Error", null);

            // Act
            var isSuccess = result.IsSuccessStatusCode();

            // Assert
            isSuccess.Should().BeFalse();
        }

        [Fact]
        public void GetResult_WithSuccessStatus_ReturnsOkObjectResult()
        {
            // Arrange
            var data = "Test Data";
            var operationResult = new OperationResult<string>(OperationStatus.Ok, "Success", data);

            // Act
            var actionResult = operationResult.GetResult();

            // Assert
            actionResult.Should().BeOfType<ObjectResult>();
            var objectResult = actionResult as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
            objectResult.Value.Should().Be(data);
        }

        [Fact]
        public void GetResult_WithNotFoundStatus_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var message = "Not Found";
            var operationResult = new OperationResult<string>(OperationStatus.NotFound, message, null);

            // Act
            var actionResult = operationResult.GetResult();

            // Assert
            actionResult.Should().BeOfType<ObjectResult>();
            var objectResult = actionResult as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
            objectResult.Value.Should().Be(message);
        }

        [Fact]
        public void GetResult_WithBadRequestStatus_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var message = "Bad Request";
            var operationResult = new OperationResult<string>(OperationStatus.BadRequest, message, null);

            // Act
            var actionResult = operationResult.GetResult();

            // Assert
            actionResult.Should().BeOfType<ObjectResult>();
            var objectResult = actionResult as ObjectResult;
            objectResult!.StatusCode.Should().Be(400);
            objectResult.Value.Should().Be(message);
        }

        [Fact]
        public void GetResult_WithCreatedStatus_ReturnsCreatedObjectResult()
        {
            // Arrange
            var data = "Created Data";
            var operationResult = new OperationResult<string>(OperationStatus.Created, "Created", data);

            // Act
            var actionResult = operationResult.GetResult();

            // Assert
            actionResult.Should().BeOfType<ObjectResult>();
            var objectResult = actionResult as ObjectResult;
            objectResult!.StatusCode.Should().Be(201);
            objectResult.Value.Should().Be(data);
        }

        [Fact]
        public void GetResult_WithConflictStatus_ReturnsConflictObjectResult()
        {
            // Arrange
            var message = "Conflict";
            var operationResult = new OperationResult<string>(OperationStatus.Conflict, message, null);

            // Act
            var actionResult = operationResult.GetResult();

            // Assert
            actionResult.Should().BeOfType<ObjectResult>();
            var objectResult = actionResult as ObjectResult;
            objectResult!.StatusCode.Should().Be(409);
            objectResult.Value.Should().Be(message);
        }

        [Theory]
        [InlineData(OperationStatus.Ok, 200)]
        [InlineData(OperationStatus.Created, 201)]
        [InlineData(OperationStatus.BadRequest, 400)]
        [InlineData(OperationStatus.NotFound, 404)]
        [InlineData(OperationStatus.Conflict, 409)]
        [InlineData(OperationStatus.InternalServerError, 500)]
        public void ToInt_ConvertsStatusToCorrectHttpCode(OperationStatus status, int expectedCode)
        {
            // Act
            var code = status.ToInt();

            // Assert
            code.Should().Be(expectedCode);
        }

        [Fact]
        public void OperationResult_WithNullMessage_HandlesCorrectly()
        {
            // Arrange & Act
            var result = new OperationResult<string>(OperationStatus.Ok, null, "data");

            // Assert
            result.Message.Should().BeNull();
            result.IsSuccessStatusCode().Should().BeTrue();
        }

        [Fact]
        public void OperationResult_WithNullData_HandlesCorrectly()
        {
            // Arrange & Act
            var result = new OperationResult<string>(OperationStatus.Ok, "Success", null);

            // Assert
            result.Result.Should().BeNull();
            result.IsSuccessStatusCode().Should().BeTrue();
        }
    }
}
