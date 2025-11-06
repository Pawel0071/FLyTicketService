using FluentAssertions;
using FLyTicketService.Extension;
using FLyTicketService.Model.Enums;
using Xunit;

namespace FlyTicketService.Tests.Extensions
{
    public class SimplyTimeZoneInfoTests
    {
        [Fact]
        public void Constructor_WithValidParameters_SetsAllProperties()
        {
            // Arrange
            var timeZone = SimplyTimeZone.CET;
            var displayName = "Central European Time";
            var offset = TimeSpan.FromHours(1);

            // Act
            var result = new SimplyTimeZoneInfo(timeZone, displayName, offset);

            // Assert
            result.TimeZone.Should().Be(timeZone);
            result.DisplayName.Should().Be(displayName);
            result.Offset.Should().Be(offset);
        }

        [Theory]
        [InlineData(SimplyTimeZone.CEST, true)]  // Central European Summer Time
        [InlineData(SimplyTimeZone.EDT, true)]   // Eastern Daylight Time
        [InlineData(SimplyTimeZone.PDT, true)]   // Pacific Daylight Time
        [InlineData(SimplyTimeZone.AEDT, true)]  // Australian Eastern Daylight Time
        public void SupportsDaylightSavingTime_WithDaylightSavingTimeZones_ReturnsTrue(SimplyTimeZone timeZone, bool expected)
        {
            // Arrange
            var timeZoneInfo = new SimplyTimeZoneInfo(timeZone, "Test", TimeSpan.Zero);

            // Act
            var result = timeZoneInfo.SupportsDaylightSavingTime();

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(SimplyTimeZone.UTC)]
        [InlineData(SimplyTimeZone.GMT)]
        [InlineData(SimplyTimeZone.CET)]
        [InlineData(SimplyTimeZone.EST)]
        [InlineData(SimplyTimeZone.PST)]
        [InlineData(SimplyTimeZone.JST)]
        [InlineData(SimplyTimeZone.AEST)]
        [InlineData(SimplyTimeZone.IST)]
        public void SupportsDaylightSavingTime_WithStandardTimeZones_ReturnsFalse(SimplyTimeZone timeZone)
        {
            // Arrange
            var timeZoneInfo = new SimplyTimeZoneInfo(timeZone, "Test", TimeSpan.Zero);

            // Act
            var result = timeZoneInfo.SupportsDaylightSavingTime();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void DisplayName_CanBeModified()
        {
            // Arrange
            var timeZoneInfo = new SimplyTimeZoneInfo(SimplyTimeZone.UTC, "Old Name", TimeSpan.Zero);
            var newDisplayName = "New Display Name";

            // Act
            timeZoneInfo.DisplayName = newDisplayName;

            // Assert
            timeZoneInfo.DisplayName.Should().Be(newDisplayName);
        }

        [Fact]
        public void Offset_CanBeModified()
        {
            // Arrange
            var timeZoneInfo = new SimplyTimeZoneInfo(SimplyTimeZone.UTC, "UTC", TimeSpan.Zero);
            var newOffset = TimeSpan.FromHours(5);

            // Act
            timeZoneInfo.Offset = newOffset;

            // Assert
            timeZoneInfo.Offset.Should().Be(newOffset);
        }

        [Fact]
        public void TimeZone_IsReadOnly()
        {
            // Arrange
            var timeZone = SimplyTimeZone.CET;
            var timeZoneInfo = new SimplyTimeZoneInfo(timeZone, "CET", TimeSpan.FromHours(1));

            // Assert
            timeZoneInfo.TimeZone.Should().Be(timeZone);
            // TimeZone property has only getter, so it cannot be modified
        }

        [Fact]
        public void Constructor_WithNegativeOffset_SetsCorrectly()
        {
            // Arrange
            var timeZone = SimplyTimeZone.EST;
            var offset = TimeSpan.FromHours(-5);

            // Act
            var result = new SimplyTimeZoneInfo(timeZone, "Eastern Standard Time", offset);

            // Assert
            result.Offset.Should().Be(offset);
            result.Offset.TotalHours.Should().Be(-5);
        }

        [Fact]
        public void Constructor_WithHalfHourOffset_SetsCorrectly()
        {
            // Arrange
            var timeZone = SimplyTimeZone.IST;
            var offset = TimeSpan.FromHours(5.5);

            // Act
            var result = new SimplyTimeZoneInfo(timeZone, "India Standard Time", offset);

            // Assert
            result.Offset.Should().Be(offset);
            result.Offset.TotalHours.Should().Be(5.5);
        }
    }
}
