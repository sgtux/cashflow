namespace FinanceApi.Infra.Repository
{
  public class UserRepository : BaseRepository<Entity.User>, IUserRepository 
  {
    public UserRepository(AppDbContext context) : base(context) { }
  }
}