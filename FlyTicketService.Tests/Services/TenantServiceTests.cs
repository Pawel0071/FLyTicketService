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
