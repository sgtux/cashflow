using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infra.Resources
{
    public class ResourceBuilder
    {
        private string resourceName;
        public ResourceBuilder(string name)
        {
            resourceName = $"Cashflow.Api.Infra.Resources.{name}";
        }

        public async Task<string> Build()
        {
            var stream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                if (Assembly.GetEntryAssembly().GetManifestResourceNames().Contains(resourceName))
                    throw new InvalidDataException($"Resource '{resourceName}' not found.");
                throw new InvalidDataException($"Resource '{resourceName}' found but can't be loaded.");
            }
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                return await reader.ReadToEndAsync();
        }
    }
}