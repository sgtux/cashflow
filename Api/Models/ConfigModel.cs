namespace Cashflow.Api.Models
{
    public class ConfigModel
    {
        public string GoogleClientId { get; set; }

        public string AuthApiUrl { get; set; }

        public string MicrosoftClientId { get; set; }

        public string MicrosoftTenantId { get; set; }

        public string MicrosoftRedirectUri { get; set; }

        public string MicrosoftAuthority { get; set; }
    }
}