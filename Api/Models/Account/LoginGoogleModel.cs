namespace Cashflow.Api.Models.Account
{
    public class LoginGoogleModel
    {
        public string IdToken { get; set; }

        public string AccessToken { get; set; }

        public bool IsValid => IdToken != null || AccessToken != null;
    }
}