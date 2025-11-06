using FluentAssertions;
using FLyTicketService.Data;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Repositories;
using FLyTicketService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FlyTicketService.Tests.Repositories
{
    public class GenericRepositoryTests : IDisposable
    {
        private readonly FLyTicketDbContext _context;
        private readonly IGenericRepository<Tenant> _repository;
        private readonly Mock<ILogger<Tenant>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public GenericRepositoryTests()
        {
            // Setup mock configuration
            _configurationMock = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("Development");
            _configurationMock.Setup(x => x.GetSection(It.IsAny<string>())).Returns(mockSection.Object);
            _configurationMock.Setup(x => x[It.IsAny<string>()]).Returns((string?)null);
            
            // Setup In-Memory Database
            var options = new DbContextOptionsBuilder<FLyTicketDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new FLyTicketDbContext(options, _configurationMock.Object);
            _loggerMock = new Mock<ILogger<Tenant>>();
            _repository = new GenericRepository<Tenant>(_context, _loggerMock.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddAsync_WithValidEntity_AddsEntityToDatabase()
        {
            // Arrange
            var tenant = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "John Doe",
                Address = "123 Main St",
                Phone = "555-1234",
                Email = "john@example.com",
                Group = TenantGroup.GroupA,
                Birthday = new DateTime(1990, 1, 15)
            };

            // Act
            var result = await _repository.AddAsync(tenant);

            // Assert
            result.Should().BeTrue();
            var savedTenant = await _context.Set<Tenant>().FindAsync(tenant.TenantId);
            savedTenant.Should().NotBeNull();
            savedTenant!.Name.Should().Be("John Doe");
            savedTenant.Email.Should().Be("john@example.com");
        }

        [Fact]
        public async Task GetAllAsync_WithMultipleEntities_ReturnsAllEntities()
        {
            // Arrange
            var tenant1 = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "Alice",
                Address = "111 First St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-25)
            };

            var tenant2 = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "Bob",
                Address = "222 Second St",
                Group = TenantGroup.GroupB,
                Birthday = DateTime.Now.AddYears(-30)
            };

            await _context.Set<Tenant>().AddRangeAsync(tenant1, tenant2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(t => t.Name == "Alice");
            result.Should().Contain(t => t.Name == "Bob");
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingId_ReturnsEntity()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant
            {
                TenantId = tenantId,
                Name = "Charlie",
                Address = "333 Third St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-28)
            };

            await _context.Set<Tenant>().AddAsync(tenant);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(tenantId);

            // Assert
            result.Should().NotBeNull();
            result!.TenantId.Should().Be(tenantId);
            result.Name.Should().Be("Charlie");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingId_ReturnsNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(nonExistingId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_WithExistingEntity_UpdatesEntity()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant
            {
                TenantId = tenantId,
                Name = "Original Name",
                Address = "Original Address",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-30)
            };

            await _context.Set<Tenant>().AddAsync(tenant);
            await _context.SaveChangesAsync();

            // Detach to simulate fresh load
            _context.Entry(tenant).State = EntityState.Detached;

            // Modify the entity
            tenant.Name = "Updated Name";
            tenant.Address = "Updated Address";

            // Act
            var result = await _repository.UpdateAsync(tenant);

            // Assert
            result.Should().BeTrue();
            var updatedTenant = await _context.Set<Tenant>().FindAsync(tenantId);
            updatedTenant.Should().NotBeNull();
            updatedTenant!.Name.Should().Be("Updated Name");
            updatedTenant.Address.Should().Be("Updated Address");
        }

        [Fact]
        public async Task RemoveAsync_WithExistingId_RemovesEntity()
        {
            // Arrange
            var tenantId = Guid.NewGuid();
            var tenant = new Tenant
            {
                TenantId = tenantId,
                Name = "To Be Deleted",
                Address = "Delete Address",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-35)
            };

            await _context.Set<Tenant>().AddAsync(tenant);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.RemoveAsync(tenantId);

            // Assert
            result.Should().BeTrue();
            var deletedTenant = await _context.Set<Tenant>().FindAsync(tenantId);
            deletedTenant.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_WithNonExistingId_ReturnsFalse()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var result = await _repository.RemoveAsync(nonExistingId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetByAsync_WithMatchingPredicate_ReturnsEntity()
        {
            // Arrange
            var tenant1 = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "David",
                Address = "444 Fourth St",
                Email = "david@example.com",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-27)
            };

            var tenant2 = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "Eve",
                Address = "555 Fifth St",
                Email = "eve@example.com",
                Group = TenantGroup.GroupB,
                Birthday = DateTime.Now.AddYears(-32)
            };

            await _context.Set<Tenant>().AddRangeAsync(tenant1, tenant2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByAsync(t => t.Email == "david@example.com");

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("David");
            result.Email.Should().Be("david@example.com");
        }

        [Fact]
        public async Task GetByAsync_WithNonMatchingPredicate_ReturnsNull()
        {
            // Arrange
            var tenant = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "Frank",
                Address = "666 Sixth St",
                Email = "frank@example.com",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-29)
            };

            await _context.Set<Tenant>().AddAsync(tenant);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByAsync(t => t.Email == "nonexistent@example.com");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task FilterByAsync_WithMatchingPredicate_ReturnsMatchingEntities()
        {
            // Arrange
            var tenant1 = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "GroupA Member 1",
                Address = "777 Seventh St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-26)
            };

            var tenant2 = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "GroupA Member 2",
                Address = "888 Eighth St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-31)
            };

            var tenant3 = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "GroupB Member",
                Address = "999 Ninth St",
                Group = TenantGroup.GroupB,
                Birthday = DateTime.Now.AddYears(-28)
            };

            await _context.Set<Tenant>().AddRangeAsync(tenant1, tenant2, tenant3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.FilterByAsync(t => t.Group == TenantGroup.GroupA);

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(t => t.Group.Should().Be(TenantGroup.GroupA));
            result.Should().Contain(t => t.Name == "GroupA Member 1");
            result.Should().Contain(t => t.Name == "GroupA Member 2");
        }

        [Fact]
        public async Task FilterByAsync_WithNonMatchingPredicate_ReturnsEmptyCollection()
        {
            // Arrange
            var tenant = new Tenant
            {
                TenantId = Guid.NewGuid(),
                Name = "Solo Tenant",
                Address = "101 Solo St",
                Group = TenantGroup.GroupA,
                Birthday = DateTime.Now.AddYears(-33)
            };

            await _context.Set<Tenant>().AddAsync(tenant);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.FilterByAsync(t => t.Name == "Non-Existent Name");

            // Assert
            result.Should().BeEmpty();
        }
    }
}
