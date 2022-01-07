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
    }
}