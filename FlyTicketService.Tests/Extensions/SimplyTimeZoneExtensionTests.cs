using FluentAssertions;
using FLyTicketService.Extension;
using FLyTicketService.Model.Enums;
using Xunit;

namespace FlyTicketService.Tests.Extensions
{
    public class SimplyTimeZoneExtensionTests
    {
        [Fact]
        public void TimeZones_StaticCollection_IsInitializedWithAllTimeZones()
        {
            // Assert
            SimplyTimeZoneExtension.TimeZones.Should().HaveCount(12);
            SimplyTimeZoneExtension.TimeZones.Should().Contain(tz => tz.TimeZone == SimplyTimeZone.UTC);
            SimplyTimeZoneExtension.TimeZones.Should().Contain(tz => tz.TimeZone == SimplyTimeZone.CET);
            SimplyTimeZoneExtension.TimeZones.Should().Contain(tz => tz.TimeZone == SimplyTimeZone.EST);
        }

        [Fact]
        public void ConvertToTargetTimeZone_DateTime_FromUTCToCET_AddsOneHour()
        {
            // Arrange
            var utcDateTime = new DateTime(2025, 1, 15, 12, 0, 0);
            var sourceTimeZone = SimplyTimeZone.UTC;
            var targetTimeZone = SimplyTimeZone.CET;

            // Act
            var result = utcDateTime.ConvertToTargetTimeZone(sourceTimeZone, targetTimeZone);

            // Assert
            result.Should().Be(new DateTime(2025, 1, 15, 13, 0, 0));
        }

        [Fact]
        public void ConvertToTargetTimeZone_DateTime_FromCETToCEST_AddsOneHour()
        {
            // Arrange
            var cetDateTime = new DateTime(2025, 6, 15, 12, 0, 0);
            var sourceTimeZone = SimplyTimeZone.CET;
            var targetTimeZone = SimplyTimeZone.CEST;

            // Act
            var result = cetDateTime.ConvertToTargetTimeZone(sourceTimeZone, targetTimeZone);

            // Assert
            result.Should().Be(new DateTime(2025, 6, 15, 13, 0, 0));
        }

        [Fact]
        public void ConvertToTargetTimeZone_DateTime_FromESTToPST_SubtractsThreeHours()
        {
            // Arrange
            var estDateTime = new DateTime(2025, 1, 15, 15, 30, 0);
            var sourceTimeZone = SimplyTimeZone.EST;
            var targetTimeZone = SimplyTimeZone.PST;

            // Act
            var result = estDateTime.ConvertToTargetTimeZone(sourceTimeZone, targetTimeZone);

            // Assert
            result.Should().Be(new DateTime(2025, 1, 15, 12, 30, 0));
        }

        [Fact]
        public void ConvertToTargetTimeZone_DateTime_FromJSTToAEST_AddsOneHour()
        {
            // Arrange
            var jstDateTime = new DateTime(2025, 1, 15, 9, 0, 0);
            var sourceTimeZone = SimplyTimeZone.JST;
            var targetTimeZone = SimplyTimeZone.AEST;

            // Act
            var result = jstDateTime.ConvertToTargetTimeZone(sourceTimeZone, targetTimeZone);

            // Assert
            result.Should().Be(new DateTime(2025, 1, 15, 10, 0, 0));
        }

        [Fact]
        public void ConvertToTargetTimeZone_DateTime_SameTimeZone_ReturnsUnchanged()
        {
            // Arrange
            var dateTime = new DateTime(2025, 1, 15, 12, 0, 0);
            var timeZone = SimplyTimeZone.UTC;

            // Act
            var result = dateTime.ConvertToTargetTimeZone(timeZone, timeZone);

            // Assert
            result.Should().Be(dateTime);
        }

        [Fact]
        public void ConvertToTargetTimeZone_DateTimeOffset_FromUTCToCET_AddsOneHour()
        {
            // Arrange
            var utcDateTime = new DateTimeOffset(2025, 1, 15, 12, 0, 0, TimeSpan.Zero);
            var sourceTimeZone = SimplyTimeZone.UTC;
            var targetTimeZone = SimplyTimeZone.CET;

            // Act
            var result = utcDateTime.ConvertToTargetTimeZone(sourceTimeZone, targetTimeZone);

            // Assert
            result.Should().Be(new DateTime(2025, 1, 15, 13, 0, 0));
        }

        [Fact]
        public void ConvertToTargetTimeZone_DateTimeOffset_FromPSTToEST_AddsThreeHours()
        {
            // Arrange
            var pstDateTime = new DateTimeOffset(2025, 1, 15, 9, 0, 0, TimeSpan.FromHours(-8));
            var sourceTimeZone = SimplyTimeZone.PST;
            var targetTimeZone = SimplyTimeZone.EST;

            // Act
            var result = pstDateTime.ConvertToTargetTimeZone(sourceTimeZone, targetTimeZone);

            // Assert
            result.Should().Be(new DateTime(2025, 1, 15, 20, 0, 0));
        }

        [Fact]
        public void ConvertToTargetTimeZone_DateTimeOffset_FromISTToGMT_SubtractsHalfHours()
        {
            // Arrange - IST is UTC+5.5, so 12:00 IST = 06:30 UTC = 06:30 GMT
            var istDateTime = new DateTimeOffset(2025, 1, 15, 12, 0, 0, TimeSpan.FromHours(5.5));
            var sourceTimeZone = SimplyTimeZone.IST;
            var targetTimeZone = SimplyTimeZone.GMT;

            // Act
            var result = istDateTime.ConvertToTargetTimeZone(sourceTimeZone, targetTimeZone);

            // Assert - Converting from IST (UTC+5.5) to GMT (UTC+0) subtracts 5.5 hours
            result.Should().Be(new DateTime(2025, 1, 15, 1, 0, 0));
        }

        [Fact]
        public void TimeZones_ContainsCorrectOffsets()
        {
            // Assert
            var utc = SimplyTimeZoneExtension.TimeZones.Find(tz => tz.TimeZone == SimplyTimeZone.UTC);
            utc.Should().NotBeNull();
            utc!.Offset.Should().Be(TimeSpan.Zero);

            var cet = SimplyTimeZoneExtension.TimeZones.Find(tz => tz.TimeZone == SimplyTimeZone.CET);
            cet.Should().NotBeNull();
            cet!.Offset.Should().Be(TimeSpan.FromHours(1));

            var est = SimplyTimeZoneExtension.TimeZones.Find(tz => tz.TimeZone == SimplyTimeZone.EST);
            est.Should().NotBeNull();
            est!.Offset.Should().Be(TimeSpan.FromHours(-5));

            var ist = SimplyTimeZoneExtension.TimeZones.Find(tz => tz.TimeZone == SimplyTimeZone.IST);
            ist.Should().NotBeNull();
            ist!.Offset.Should().Be(TimeSpan.FromHours(5.5));
        }
    }
}
