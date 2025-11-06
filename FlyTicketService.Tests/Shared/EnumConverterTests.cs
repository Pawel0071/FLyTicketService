using FluentAssertions;
using FLyTicketService.Model.Enums;
using FLyTicketService.Shared;
using System.Text.Json;
using Xunit;

namespace FlyTicketService.Tests.Shared
{
    public class EnumConverterTests
    {
        [Fact]
        public void Read_ValidTenantGroupString_ReturnsCorrectEnum()
        {
            // Arrange
            var json = "\"GroupA\"";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<TenantGroup>());

            // Act
            var result = JsonSerializer.Deserialize<TenantGroup>(json, options);

            // Assert
            result.Should().Be(TenantGroup.GroupA);
        }

        [Fact]
        public void Read_ValidTicketStatusString_ReturnsCorrectEnum()
        {
            // Arrange
            var json = "\"Reserved\"";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<TicketStatus>());

            // Act
            var result = JsonSerializer.Deserialize<TicketStatus>(json, options);

            // Assert
            result.Should().Be(TicketStatus.Reserved);
        }

        [Fact]
        public void Read_ValidSeatClassString_ReturnsCorrectEnum()
        {
            // Arrange
            var json = "\"Business\"";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<SeatClass>());

            // Act
            var result = JsonSerializer.Deserialize<SeatClass>(json, options);

            // Assert
            result.Should().Be(SeatClass.Business);
        }

        [Fact]
        public void Read_InvalidEnumString_ThrowsJsonException()
        {
            // Arrange
            var json = "\"InvalidValue\"";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<TenantGroup>());

            // Act
            Action act = () => JsonSerializer.Deserialize<TenantGroup>(json, options);

            // Assert
            act.Should().Throw<JsonException>()
                .WithMessage("Invalid value for TenantGroup: InvalidValue");
        }

        [Fact]
        public void Read_EmptyString_ThrowsJsonException()
        {
            // Arrange
            var json = "\"\"";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<TicketStatus>());

            // Act
            Action act = () => JsonSerializer.Deserialize<TicketStatus>(json, options);

            // Assert
            act.Should().Throw<JsonException>();
        }

        [Fact]
        public void Write_TenantGroupEnum_WritesCorrectString()
        {
            // Arrange
            var enumValue = TenantGroup.GroupB;
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<TenantGroup>());

            // Act
            var json = JsonSerializer.Serialize(enumValue, options);

            // Assert
            json.Should().Be("\"GroupB\"");
        }

        [Fact]
        public void Write_TicketStatusEnum_WritesCorrectString()
        {
            // Arrange
            var enumValue = TicketStatus.Sold;
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<TicketStatus>());

            // Act
            var json = JsonSerializer.Serialize(enumValue, options);

            // Assert
            json.Should().Be("\"Sold\"");
        }

        [Fact]
        public void Write_SeatClassEnum_WritesCorrectString()
        {
            // Arrange
            var enumValue = SeatClass.First;
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<SeatClass>());

            // Act
            var json = JsonSerializer.Serialize(enumValue, options);

            // Assert
            json.Should().Be("\"First\"");
        }

        [Fact]
        public void RoundTrip_TenantGroup_PreservesValue()
        {
            // Arrange
            var original = TenantGroup.GroupA;
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<TenantGroup>());

            // Act
            var json = JsonSerializer.Serialize(original, options);
            var result = JsonSerializer.Deserialize<TenantGroup>(json, options);

            // Assert
            result.Should().Be(original);
        }

        [Fact]
        public void RoundTrip_TicketStatus_PreservesValue()
        {
            // Arrange
            var original = TicketStatus.Sold;
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<TicketStatus>());

            // Act
            var json = JsonSerializer.Serialize(original, options);
            var result = JsonSerializer.Deserialize<TicketStatus>(json, options);

            // Assert
            result.Should().Be(original);
        }

        [Fact]
        public void Read_DaysOfWeekMask_ReturnsCorrectEnum()
        {
            // Arrange
            var json = "\"Monday\"";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EnumConverter<DaysOfWeekMask>());

            // Act
            var result = JsonSerializer.Deserialize<DaysOfWeekMask>(json, options);

            // Assert
            result.Should().Be(DaysOfWeekMask.Monday);
        }
    }
}
