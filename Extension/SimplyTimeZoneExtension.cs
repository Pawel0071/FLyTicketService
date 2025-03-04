using FLyTicketService.Model.Enums;

namespace FLyTicketService.Extension
{
    public static class SimplyTimeZoneExtension
    {
        public static List<SimplyTimeZoneInfo> TimeZones { get; internal set; }

        static SimplyTimeZoneExtension()
        {
            TimeZones = new List<SimplyTimeZoneInfo>
            {
                new SimplyTimeZoneInfo(SimplyTimeZone.UTC, "Coordinated Universal Time", TimeSpan.Zero),
                new SimplyTimeZoneInfo(SimplyTimeZone.GMT, "Greenwich Mean Time", TimeSpan.Zero),
                new SimplyTimeZoneInfo(SimplyTimeZone.CET, "Central European Time", TimeSpan.FromHours(1)),
                new SimplyTimeZoneInfo(SimplyTimeZone.CEST, "Central European Summer Time", TimeSpan.FromHours(2)),
                new SimplyTimeZoneInfo(SimplyTimeZone.EST, "Eastern Standard Time", TimeSpan.FromHours(-5)),
                new SimplyTimeZoneInfo(SimplyTimeZone.EDT, "Eastern Daylight Time", TimeSpan.FromHours(-4)),
                new SimplyTimeZoneInfo(SimplyTimeZone.PST, "Pacific Standard Time", TimeSpan.FromHours(-8)),
                new SimplyTimeZoneInfo (SimplyTimeZone.PDT, "Pacific Daylight Time", TimeSpan.FromHours(-7)),
                new SimplyTimeZoneInfo (SimplyTimeZone.JST, "Japan Standard Time", TimeSpan.FromHours(9)),
                new SimplyTimeZoneInfo (SimplyTimeZone.AEST, "Australian Eastern Standard Time", TimeSpan.FromHours(10)),
                new SimplyTimeZoneInfo (SimplyTimeZone.AEDT, "Australian Eastern Daylight Time", TimeSpan.FromHours(11)),
                new SimplyTimeZoneInfo (SimplyTimeZone.IST, "India Standard Time", TimeSpan.FromHours(5.5))
            };

        }
    }
}
