using System.Collections.Generic;

namespace Cashflow.Api.Models
{
    public class SecurityOAuthKeyModel
    {
        public List<SecurityOauthKeyPartsModel> Keys { get; set; }
    }

    public class SecurityOauthKeyPartsModel
    {
        public string Kty { get; set; }

        public string E { get; set; }

        public string Use { get; set; }

        public string Kid { get; set; }

        public string Alg { get; set; }

        public string N { get; set; }
    }
}