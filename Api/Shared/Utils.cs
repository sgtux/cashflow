using System;
using System.Linq;
using BCrypt.Net;

namespace Cashflow.Api.Shared
{
    public static class Utils
    {
        public static string PasswordHash(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12, HashType.SHA512);

        public static bool PasswordHashVarify(string password, string hash) => BCrypt.Net.BCrypt.EnhancedVerify(password, hash, HashType.SHA512);

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