using System.Linq;
using FinanceApi.Infra.Entity;

namespace FinanceApi.Infra.Repository
{
  /// User repository
  public class UserRepository : BaseRepository<User>, IUserRepository
  {
    /// Constructor
    public UserRepository(AppDbContext context) : base(context) { }

    /// Verify if user exists
    public bool UserExists(int userId) => _context.User.Any(p => p.Id == userId);

    /// Find by name or email
    public User FindByNameEmail(string name, string email) => _context.User.FirstOrDefault(p => p.Email == email || p.Name == name);
    public User FindByEmail(string email) => _context.User.FirstOrDefault(p => p.Email == email);
  }
}