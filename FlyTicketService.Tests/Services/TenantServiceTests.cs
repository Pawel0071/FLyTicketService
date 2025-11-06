using FluentAssertions;
using FLyTicketService.DTO;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories.Interfaces;
using FLyTicketService.Service;
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
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().NotBeNull();
            result.Data!.TenantId.Should().Be(tenantId);
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
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.NotFound);
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task CreateTenantAsync_WithValidData_CreatesSuccessfully()
        {
            // Arrange
            var tenantDto = CreateTestTenantDto();

            _tenantRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Tenant>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.CreateTenantAsync(tenantDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().NotBeNull();
            _tenantRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Tenant>()), Times.Once);
        }

        [Fact]
        public async Task CreateTenantAsync_WhenRepositoryFails_ReturnsError()
        {
            // Arrange
            var tenantDto = CreateTestTenantDto();

            _tenantRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Tenant>()))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.CreateTenantAsync(tenantDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.InternalServerError);
        }

        [Fact]
        public async Task UpdateTenantAsync_WithValidData_UpdatesSuccessfully()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenantDto = CreateTestTenantDto();
            var existingTenant = CreateTestTenant(tenantId);

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync(existingTenant);

            _tenantRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Tenant>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.UpdateTenantAsync(tenantId, tenantDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            _tenantRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Tenant>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTenantAsync_WhenTenantNotFound_ReturnsNotFound()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenantDto = CreateTestTenantDto();

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync((Tenant?)null);

            // Act
            var result = await _sut.UpdateTenantAsync(tenantId, tenantDto);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.NotFound);
        }

        [Fact]
        public async Task DeleteTenantAsync_WhenTenantExists_DeletesSuccessfully()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = CreateTestTenant(tenantId);

            _tenantRepositoryMock
                .Setup(x => x.GetByIdAsync(tenantId))
                .ReturnsAsync(tenant);

            _tenantRepositoryMock
                .Setup(x => x.DeleteAsync(tenant))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteTenantAsync(tenantId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllTenantsAsync_ReturnsAllTenants()
        {
            // Arrange
            var tenants = new List<Tenant>
            {
                CreateTestTenant(Guid.NewGuid()),
                CreateTestTenant(Guid.NewGuid())
            };

            _tenantRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(tenants);

            // Act
            var result = await _sut.GetAllTenantsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(FLyTicketService.Shared.OperationStatus.Ok);
            result.Data.Should().HaveCount(2);
        }

        private Tenant CreateTestTenant(Guid tenantId)
        {
            return new Tenant
            {
                TenantId = tenantId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@test.com",
                PhoneNumber = "1234567890",
                Group = TenantGroup.GroupA
            };
        }

        private TenantDTO CreateTestTenantDto()
        {
            return new TenantDTO
            {
                TenantId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@test.com",
                PhoneNumber = "0987654321",
                Group = TenantGroup.GroupB
            };
        }
    }
}
