using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Api.Extensions
{
  public static class StringExtensions
  {
    public static async Task<string> GetResource(this string resourceName)
    {
      var resourceFullName = $"Cashflow.Api.Infra.Resources.{resourceName}";
      var stream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceFullName);
      if (stream == null)
      {
        if (Assembly.GetEntryAssembly().GetManifestResourceNames().Contains(resourceFullName))
          throw new InvalidDataException("Resource not found.");
        throw new InvalidDataException("Resource found but can't be loaded.");
      }
      using (var reader = new StreamReader(stream, Encoding.UTF8))
        return await reader.ReadToEndAsync();
    }
  }
}