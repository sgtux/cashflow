using System;

namespace Cashflow.Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FixFirstDayInMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

        public static DateTime FixLastDayInMonth(this DateTime date) => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

        public static DateTime FixStartTimeFilter(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

        public static DateTime FixEndTimeFilter(this DateTime date)
        {
            var result = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            return result.AddDays(1).AddMilliseconds(-1);
        }
    }
}