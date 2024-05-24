using System;

namespace Cashflow.Api.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime CurrentDate
        {
            get
            {
                DateTime dateTime = DateTime.UtcNow;
                TimeZoneInfo hrBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, hrBrasilia);
            }
        }
    }
}