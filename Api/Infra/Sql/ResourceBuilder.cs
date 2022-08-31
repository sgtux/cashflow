using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cashflow.Api.Infra.Sql
{
    public class ResourceBuilder
    {
        private readonly string _resourceName;

        public ResourceBuilder(string name) => _resourceName = $"Cashflow.Api.Infra.Sql.{name}";

        public async Task<string> Build()
        {
            var stream = typeof(ResourceBuilder).Assembly.GetManifestResourceStream(_resourceName);
            if (stream == null)
            {
                if (Assembly.GetEntryAssembly().GetManifestResourceNames().Contains(_resourceName))
                    throw new InvalidDataException($"Resource '{_resourceName}' not found.");
                throw new InvalidDataException($"Resource '{_resourceName}' found but can't be loaded.");
            }
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                return await reader.ReadToEndAsync();
        }
    }
}