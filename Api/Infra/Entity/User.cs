using System;
using System.Collections.Generic;

namespace Cashflow.Api.Infra.Entity
{
    public class User
    {
        public int Id { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}