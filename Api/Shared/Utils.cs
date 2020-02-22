using System.Security.Cryptography;
using System.Text;

namespace Cashflow.Api.Shared
{
  /// Utilities
  public static class Utils
  {
    /// Generate sha1 from input
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

    /// Mapper same properties between objects
    public static T MapperTo<T>(this T source, T target)
    {
      foreach (var prop in typeof(T).GetProperties())
      {
        if (prop.GetMethod != null && prop.SetMethod != null)
          prop.SetValue(target, prop.GetValue(source));
      }
      return target;
    }
  }
}