using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cashflow.Api.Shared
{
    public static class Utils
    {
        public static string Sha1(string input)
        {
            var enc = Encoding.GetEncoding(0);
            byte[] buffer = enc.GetBytes(input);
            var sha1 = SHA1.Create();
            var hash = BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", "");
            return hash;
        }

        public static U Map<T, U>(this T source)
        {
            var result = default(U);
            if (source != null)
            {
                var targetType = typeof(U);
                result = (U)Activator.CreateInstance(targetType);
                var targetProps = targetType.GetProperties();
                foreach (var sourceProp in typeof(T).GetProperties())
                {
                    var prop = targetProps.FirstOrDefault(p => p.Name == sourceProp.Name);
                    if (prop?.SetMethod != null && sourceProp.GetMethod != null)
                        prop.SetValue(result, sourceProp.GetValue(source));
                }
            }
            return result;
        }

        public static U Map<T, U>(this T source, U target)
        {
            if (source != null && target != null)
            {
                var targetType = typeof(U);
                var targetProps = targetType.GetProperties();
                foreach (var sourceProp in typeof(T).GetProperties())
                {
                    var prop = targetProps.FirstOrDefault(p => p.Name == sourceProp.Name);
                    if (prop?.SetMethod != null && sourceProp.GetMethod != null)
                        prop.SetValue(target, sourceProp.GetValue(source));
                }
            }
            return target;
        }

        public static DateTime CreateEndDate(int year, int month) => new DateTime(year, month, DateTime.DaysInMonth(year, month));

        public static DateTime CreateEndDate(this DateTime date) => CreateEndDate(date.Year, date.Month);

        public static DateTime FixStartTimeFilter(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime FixEndTimeFilter(this DateTime date)
        {
            var result = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            return result.AddDays(1).AddMilliseconds(-1);
        }
    }
}