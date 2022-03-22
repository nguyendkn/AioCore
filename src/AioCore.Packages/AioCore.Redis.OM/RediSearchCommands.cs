using AioCore.Redis.OM.Contracts;
using AioCore.Redis.OM.Modeling;
using AioCore.Redis.OM.Searching;
using AioCore.Redis.OM.Searching.Query;

namespace AioCore.Redis.OM
{
    public static class RediSearchCommands
    {
        public static SearchResponse<T> Search<T>(this IRedisConnection connection, RedisQuery query)
            where T : notnull
        {
            var args = query.SerializeQuery();
            var res = connection.Execute("FT.SEARCH", args);
            return new SearchResponse<T>(res);
        }

        public static async Task<SearchResponse<T>> SearchAsync<T>(this IRedisConnection connection, RedisQuery query)
            where T : notnull
        {
            var args = query.SerializeQuery();
            var res = await connection.ExecuteAsync("FT.SEARCH", args);
            return new SearchResponse<T>(res);
        }

        public static bool CreateIndex(this IRedisConnection connection, Type type)
        {
            try
            {
                var serializedParams = type.SerializeIndex();
                connection.Execute("FT.CREATE", serializedParams);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Index already exists"))
                {
                    return false;
                }

                throw;
            }
        }

        public static async Task<bool> CreateIndexAsync(this IRedisConnection connection, Type type)
        {
            try
            {
                var serializedParams = type.SerializeIndex();
                await connection.ExecuteAsync("FT.CREATE", serializedParams);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Index already exists"))
                {
                    return false;
                }

                throw;
            }
        }

        public static async Task<bool> DropIndexAsync(this IRedisConnection connection, Type type)
        {
            try
            {
                var indexName = type.SerializeIndex().First();
                await connection.ExecuteAsync("FT.DROPINDEX", indexName);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Unknown Index name"))
                {
                    return false;
                }

                throw;
            }
        }

        public static bool DropIndex(this IRedisConnection connection, Type type)
        {
            try
            {
                var indexName = type.SerializeIndex().First();
                connection.Execute("FT.DROPINDEX", indexName);
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Unknown Index name"))
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Deletes an index. And drops associated records.
        /// </summary>
        /// <param name="connection">the connection.</param>
        /// <param name="type">the type to drop the index for.</param>
        /// <returns>whether the index was dropped or not.</returns>
        public static bool DropIndexAndAssociatedRecords(this IRedisConnection connection, Type type)
        {
            try
            {
                var indexName = type.SerializeIndex().First();
                connection.Execute("FT.DROPINDEX", indexName, "DD");
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Unknown Index name"))
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Search redis with the given query.
        /// </summary>
        /// <param name="connection">the connection to redis.</param>
        /// <param name="query">the query to use in the search.</param>
        /// <returns>a Redis reply.</returns>
        internal static RedisReply SearchRawResult(this IRedisConnection connection, RedisQuery query)
        {
            var args = query.SerializeQuery();
            return connection.Execute("FT.SEARCH", args);
        }
    }
}