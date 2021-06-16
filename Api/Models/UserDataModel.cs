using Cashflow.Api.Infra.Entity;

namespace Cashflow.Api.Models
{
    public class UserDataModel
    {
        public UserDataModel() { }

        public UserDataModel(User user)
        {
            Id = user.Id;
            NickName = user.NickName;
        }

        public int Id { get; set; }

        public string NickName { get; set; }
    }
}