using System.Threading.Tasks;

namespace Cashflow.Api.Contracts
{
    public interface ISystemParameterRepository
    {
        Task<int> MaximumSystemUsers();
    }
}