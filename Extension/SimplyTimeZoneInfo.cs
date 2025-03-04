using FLyTicketService.Model.Enums;

namespace FLyTicketService.Extension
{
    public class SimplyTimeZoneInfo
    {
        #region Properties

        private SimplyTimeZone TimeZone { get; }
        private string DisplayName { get; set; }
        private TimeSpan Offset { get; set; }

        #endregion

        #region Constructors

        public SimplyTimeZoneInfo(SimplyTimeZone timeZone, string displayName, TimeSpan offset)
        {
            TimeZone = timeZone;
            DisplayName = displayName;
            Offset = offset;
        }

        #endregion

        #region Methods

        public bool SupportsDaylightSavingTime()
        {
            return TimeZone is SimplyTimeZone.CEST or SimplyTimeZone.EDT or SimplyTimeZone.PDT or SimplyTimeZone.AEDT;
        }

        #endregion
    }
}