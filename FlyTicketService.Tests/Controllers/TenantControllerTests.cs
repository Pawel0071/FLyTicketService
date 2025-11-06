using FluentAssertions;
using FLyTicketService.Controllers;
using FLyTicketService.DTO;
using FLyTicketService.Model.Enums;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Controllers
{
    public class TenantControllerTests
    {
        private readonly Mock<ITenantService> _tenantServiceMock;
        private readonly TenantController _controller;

        public TenantControllerTests()
        {
            _tenantServiceMock = new Mock<ITenantService>();
            _controller = new TenantController(_tenantServiceMock.Object);
        }

        [Fact]
        public async Task AddTenant_WithValidData_ReturnsOk()
        {
            // Arrange
            var tenantDto = new TenantDTO
            {
                Name = "John Doe",
                Address = "123 Main St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Parse("1990-01-01")
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Tenant added successfully",
                true);

            _tenantServiceMock
                .Setup(x => x.AddTenantAsync(tenantDto))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.AddTenant(tenantDto);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateTenant_WithValidData_ReturnsOk()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenantDto = new TenantDTO
            {
                Name = "Updated Name",
                Address = "456 New St",
                Group = TenantGroup.GroupB,
                Birthday = DateTime.Parse("1985-05-15")
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Tenant updated successfully",
                true);

            _tenantServiceMock
                .Setup(x => x.UpdateTenantAsync(tenantId, tenantDto))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.UpdateTenant(tenantId, tenantDto);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateTenant_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenantDto = new TenantDTO
            {
                Name = "Updated Name",
                Address = "456 New St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Parse("1985-05-15")
            };

            var operationResult = new OperationResult<bool>(
                OperationStatus.NotFound,
                "Tenant not found",
                false);

            _tenantServiceMock
                .Setup(x => x.UpdateTenantAsync(tenantId, tenantDto))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.UpdateTenant(tenantId, tenantDto);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DeleteTenant_WithValidId_ReturnsOk()
        {
            // Arrange
            var tenantId = Guid.NewGuid();

            var operationResult = new OperationResult<bool>(
                OperationStatus.Ok,
                "Tenant deleted successfully",
                true);

            _tenantServiceMock
                .Setup(x => x.DeleteTenantAsync(tenantId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.DeleteTenant(tenantId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DeleteTenant_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var tenantId = Guid.NewGuid();

            var operationResult = new OperationResult<bool>(
                OperationStatus.NotFound,
                "Tenant not found",
                false);

            _tenantServiceMock
                .Setup(x => x.DeleteTenantAsync(tenantId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.DeleteTenant(tenantId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetTenant_WithValidId_ReturnsOk()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenantDto = new TenantDTO
            {
                TenantId = tenantId,
                Name = "John Doe",
                Address = "123 Main St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Parse("1990-01-01")
            };

            var operationResult = new OperationResult<TenantDTO>(
                OperationStatus.Ok,
                "Tenant found",
                tenantDto);

            _tenantServiceMock
                .Setup(x => x.GetTenantAsync(tenantId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetTenant(tenantId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetTenant_WhenNotFound_ReturnsNotFound()
        {
            // Arrange
            var tenantId = Guid.NewGuid();

            var operationResult = new OperationResult<TenantDTO>(
                OperationStatus.NotFound,
                "Tenant not found",
                null!);

            _tenantServiceMock
                .Setup(x => x.GetTenantAsync(tenantId))
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetTenant(tenantId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetTenants_ReturnsOkWithList()
        {
            // Arrange
            var tenants = new List<TenantDTO>
            {
                new TenantDTO 
                { 
                    TenantId = Guid.NewGuid(), 
                    Name = "John Doe", 
                    Address = "123 Main St",
                    Group = TenantGroup.GroupA,
                    Birthday = DateTime.Parse("1990-01-01")
                },
                new TenantDTO 
                { 
                    TenantId = Guid.NewGuid(), 
                    Name = "Jane Smith", 
                    Address = "456 Oak Ave",
                    Group = TenantGroup.GroupB,
                    Birthday = DateTime.Parse("1985-05-15")
                }
            };

            var operationResult = new OperationResult<IEnumerable<TenantDTO>>(
                OperationStatus.Ok,
                "Tenants retrieved",
                tenants);

            _tenantServiceMock
                .Setup(x => x.GetTenantsAsync())
                .ReturnsAsync(operationResult);

            // Act
            var result = await _controller.GetTenants();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var objectResult = result as ObjectResult;
            objectResult!.StatusCode.Should().Be(200);
        }
    }
}
