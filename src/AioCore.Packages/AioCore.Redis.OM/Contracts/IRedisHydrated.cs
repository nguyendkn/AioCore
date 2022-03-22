namespace AioCore.Redis.OM.Contracts
{
    public interface IRedisHydrated
    {
        void Hydrate(IDictionary<string, string> dict);

        IDictionary<string, string> BuildHashSet();
    }
}