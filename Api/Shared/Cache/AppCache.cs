namespace Cashflow.Api.Shared.Cache
{
    public class AppCache
    {
        public ProjectionCache Projection { get; }

        public HomeCache Home { get; }

        public AppCache(ProjectionCache projectionCache, HomeCache homeCache)
        {
            Projection = projectionCache;
            Home = homeCache;
        }

        public void Clear(int userId)
        {
            Projection.Clear(userId);
            Home.Clear(userId);
        }
    }
}