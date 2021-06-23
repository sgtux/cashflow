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
            byte[] data = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
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
    }
}