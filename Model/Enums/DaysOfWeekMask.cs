﻿namespace FLyTicketService.Model.Enums
{
    [Flags]
    public enum DaysOfWeekMask
    {
        None = 0,
        Monday = 1 << 0,
        Tuesday = 1 << 1,
        Wednesday = 1 << 2,
        Thursday = 1 << 3,
        Friday = 1 << 4,
        Saturday = 1 << 5,
        Sunday = 1 << 6,
        AllDays = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday
    }
}
