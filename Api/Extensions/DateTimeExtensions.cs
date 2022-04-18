using System;

namespace Cashflow.Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FixFirstDayInMonth(this DateTime date) => new DateTime(date.Year, date.Month, 1);

        public static DateTime FixLastDayInMonth(this DateTime date) => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)).FixEndTimeFilter();

        public static DateTime FixStartTimeFilter(this DateTime date) => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

        public static DateTime? FixStartTimeFilter(this DateTime? date) => date == null ? null : FixStartTimeFilter(date.Value);

        public static DateTime FixEndTimeFilter(this DateTime date)
        {
            var result = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            return result.AddDays(1).AddMilliseconds(-1);
        }

        public static DateTime? FixEndTimeFilter(this DateTime? date) => date == null ? null : FixEndTimeFilter(date.Value);

        public static bool SameMonthYear(this DateTime date, DateTime compareTo) => date.Year == compareTo.Year && date.Month == compareTo.Month;
    }
}