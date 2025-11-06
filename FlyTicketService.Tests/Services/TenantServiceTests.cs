using FluentAssertions;
using FLyTicketService.DTO;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Services;
using FLyTicketService.Service.Interfaces;
using FLyTicketService.Shared;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Services
{
    public class TenantServiceTests
    {
        private readonly Mock<IGenericRepository<Tenant>> _tenantRepositoryMock;
        private readonly Mock<ILogger<TenantService>> _loggerMock;
        private readonly TenantService _sut;

        public TenantServiceTests()
        {
            _tenantRepositoryMock = new Mock<IGenericRepository<Tenant>>();
            _loggerMock = new Mock<ILogger<TenantService>>();
            _sut = new TenantService(_tenantRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetTenantAsync_WhenTenantExists_ReturnsTenant()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = CreateTestTenant(tenantId);

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync(tenant);

            // Act
            var result = await _sut.GetTenantAsync(tenantId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().NotBeNull();
            result.Result!.TenantId.Should().Be(tenantId);
        }

        [Fact]
        public async Task GetTenantAsync_WhenTenantDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var tenantId = Guid.NewGuid();

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync((Tenant?)null);

            // Act
            var result = await _sut.GetTenantAsync(tenantId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.NotFound);
        }

        [Fact]
        public async Task AddTenantAsync_WithValidTenant_ReturnsCreated()
        {
            // Arrange
            var tenantDto = CreateTestTenantDto();

            _tenantRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Tenant>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.AddTenantAsync(tenantDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Created);
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task GetTenantsAsync_ReturnsAllTenants()
        {
            // Arrange
            var tenants = new List<Tenant>
            {
                CreateTestTenant(),
                CreateTestTenant(),
                CreateTestTenant()
            };

            _tenantRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(tenants);

            // Act
            var result = await _sut.GetTenantsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().HaveCount(3);
        }

        [Fact]
        public async Task UpdateTenantAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var existingTenant = CreateTestTenant(tenantId);
            var updateDto = new TenantDTO
            {
                TenantId = tenantId,
                Name = "Jane Doe",
                Address = "456 Oak St",
                Group = TenantGroup.GroupB,
                Birthday = DateTime.Now.AddYears(-25),
                Phone = "987-654-3210",
                Email = "jane@example.com"
            };

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync(existingTenant);

            _tenantRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Tenant>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.UpdateTenantAsync(tenantId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Ok);
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateTenantAsync_WhenTenantDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var updateDto = CreateTestTenantDto();

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync((Tenant?)null);

            // Act
            var result = await _sut.UpdateTenantAsync(tenantId, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.NotFound);
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateTenantAsync_WithNullTenant_ReturnsBadRequest()
        {
            // Arrange
            var tenantId = Guid.NewGuid();

            // Act
            var result = await _sut.UpdateTenantAsync(tenantId, null);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.BadRequest);
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteTenantAsync_WithValidId_ReturnsSuccess()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var existingTenant = CreateTestTenant(tenantId);

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync(existingTenant);

            _tenantRepositoryMock
                .Setup(x => x.RemoveAsync(tenantId))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteTenantAsync(tenantId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.Created);
            result.Result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTenantAsync_WhenTenantDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var tenantId = Guid.NewGuid();

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync((Tenant?)null);

            // Act
            var result = await _sut.DeleteTenantAsync(tenantId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.NotFound);
            result.Result.Should().BeFalse();
        }

        [Fact]
        public async Task AddTenantAsync_WithNullTenant_ReturnsBadRequest()
        {
            // Act
            var result = await _sut.AddTenantAsync(null);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(OperationStatus.BadRequest);
            result.Result.Should().BeFalse();
        }

        private Tenant CreateTestTenant(Guid? tenantId = null)
        {
            return new Tenant
            {
                TenantId = tenantId ?? Guid.NewGuid(),
                Name = "John Doe",
                Address = "123 Main St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-30),
                Phone = "123-456-7890",
                Email = "john@example.com"
            };
        }

        private TenantDTO CreateTestTenantDto()
        {
            return new TenantDTO
            {
                TenantId = Guid.NewGuid(),
                Name = "John Doe",
                Address = "123 Main St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-30),
                Phone = "123-456-7890",
                Email = "john@example.com"
            };
        }
    }
}
