using System.Linq.Expressions;
using AioCore.Redis.OM.Modeling;

namespace AioCore.Redis.OM.Searching
{
    public interface IRedisCollection<T> : IOrderedQueryable<T>, IAsyncEnumerable<T>
    {
        RedisCollectionStateManager StateManager { get; }

        int ChunkSize { get; }

        void Save();

        ValueTask SaveAsync();

        string Insert(T item);

        Task<string> InsertAsync(T item);

        Task<T> FindByIdAsync(string id);

        T FindById(string id);


        bool Any(Expression<Func<T, bool>> expression);
        
        Task Update(T item);

        Task Delete(T item);
    }
}