using System.Text.Json;
using System.Text.Json.Serialization;
using AioCore.Redis.OM.Contracts;
using AioCore.Redis.OM.Modeling;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AioCore.Redis.OM
{
    public static class RedisCommands
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        static RedisCommands()
        {
            Options.Converters.Add(new DateTimeJsonConverter());
        }


        public static async Task<T> SetAsync<T>(this IRedisConnection connection, object obj)
        {
            var id = obj.SetId();
            var type = obj.GetType();
            if (Attribute.GetCustomAttribute(type, typeof(DocumentAttribute)) is not DocumentAttribute attr ||
                attr.StorageType == StorageType.Hash)
            {
                if (obj is IRedisHydrated redisHydrated)
                {
                    await connection.HSetAsync(id, redisHydrated.BuildHashSet().ToArray());
                }
                else
                {
                    await connection.HSetAsync(id, obj.BuildHashSet().ToArray()!);
                }
            }
            else
            {
                await connection.JsonSetAsync(id, ".", obj);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj)) ?? default!;
        }


        private static async Task<int> HSetAsync(this IRedisConnection connection, string key,
            params KeyValuePair<string, string>[] fieldValues)
        {
            var args = new List<string> {key};
            foreach (var (property, value) in fieldValues)
            {
                args.Add(property);
                args.Add(value);
            }

            return await connection.ExecuteAsync("HSET", args.ToArray());
        }


        public static async Task<bool> JsonSetAsync(this IRedisConnection connection, string key, string path,
            string json)
        {
            var result = await connection.ExecuteAsync("JSON.SET", key, path, json);
            return result == "OK";
        }


        public static async Task<bool> JsonSetAsync(this IRedisConnection connection, string key, string path,
            object obj)
        {
            var json = JsonSerializer.Serialize(obj, Options);
            var result = await connection.ExecuteAsync("JSON.SET", key, path, json);
            return result == "OK";
        }


        public static int HSet(this IRedisConnection connection, string key,
            params KeyValuePair<string, string>[] fieldValues)
        {
            var args = new List<string> {key};
            foreach (var (s, value) in fieldValues)
            {
                args.Add(s);
                args.Add(value);
            }

            return (int) connection.Execute("HSET", args.ToArray());
        }


        public static bool JsonSet(this IRedisConnection connection, string key, string path, string json)
        {
            var args = new[] {key, path, json};
            var result = connection.Execute("JSON.SET", args);
            return result == "OK";
        }


        public static bool JsonSet(this IRedisConnection connection, string key, string path, object obj)
        {
            var json = JsonSerializer.Serialize(obj, Options);
            var args = new[] {key, path, json};
            var result = connection.Execute("JSON.SET", args);
            return result == "OK";
        }


        public static string Set(this IRedisConnection connection, object obj)
        {
            var id = obj.SetId();
            var type = obj.GetType();
            if (Attribute.GetCustomAttribute(type, typeof(DocumentAttribute)) is not DocumentAttribute attr ||
                attr.StorageType == StorageType.Hash)
            {
                if (obj is IRedisHydrated redisHydrated)
                {
                    connection.HSet(id, redisHydrated.BuildHashSet().ToArray()!);
                }
                else
                {
                    connection.HSet(id, obj.BuildHashSet().ToArray()!);
                }
            }
            else
            {
                connection.JsonSet(id, ".", obj);
            }

            return id;
        }


        public static T Get<T>(this IRedisConnection connection, string keyName)
            where T : notnull
        {
            var type = typeof(T);
            if (Attribute.GetCustomAttribute(type, typeof(DocumentAttribute)) is DocumentAttribute attr &&
                attr.StorageType != StorageType.Hash) return connection.JsonGet<T>(keyName, ".");
            var dict = connection.HGetAll(keyName);
            return RedisObjectHandler.FromHashSet<T>(dict);
        }


        public static async ValueTask<T> GetAsync<T>(this IRedisConnection connection, string keyName)
            where T : notnull
        {
            var type = typeof(T);
            if (Attribute.GetCustomAttribute(type, typeof(DocumentAttribute)) is DocumentAttribute attr &&
                attr.StorageType != StorageType.Hash) return connection.JsonGet<T>(keyName, ".");
            var dict = await connection.HGetAllAsync(keyName);
            return RedisObjectHandler.FromHashSet<T>(dict);
        }


        public static T JsonGet<T>(this IRedisConnection connection, string key, params string[] paths)
        {
            var args = new List<string> {key};
            args.AddRange(paths);
            var res = connection.Execute("JSON.GET", args.ToArray());
            // var res = connection.Execute("JSON.GET", key);
            return JsonSerializer.Deserialize<T>(res, Options) ?? default!;
        }


        public static IDictionary<string, string> HGetAll(this IRedisConnection connection, string keyName)
        {
            var ret = new Dictionary<string, string>();
            var res = connection.Execute("HGETALL", keyName).ToArray();
            for (var i = 0; i < res.Length; i += 2)
            {
                ret.Add(res[i], res[i + 1]);
            }

            return ret;
        }


        public static async Task<IDictionary<string, string>> HGetAllAsync(this IRedisConnection connection,
            string keyName)
        {
            var ret = new Dictionary<string, string>();
            var res = (await connection.ExecuteAsync("HGETALL", keyName)).ToArray();
            for (var i = 0; i < res.Length; i += 2)
            {
                ret.Add(res[i], res[i + 1]);
            }

            return ret;
        }


        public static async Task<int?> CreateAndEvalAsync(this IRedisConnection connection, string scriptName,
            string[] keys, IEnumerable<string> argv, string fullScript = "")
        {
            if (!Scripts.ShaCollection.ContainsKey(scriptName))
            {
                string sha;
                if (Scripts.ScriptCollection.ContainsKey(scriptName))
                {
                    sha = await connection.ExecuteAsync("SCRIPT",
                        "LOAD", Scripts.ScriptCollection[scriptName]);
                }
                else if (!string.IsNullOrEmpty(fullScript))
                {
                    sha = await connection.ExecuteAsync("SCRIPT", "LOAD", fullScript);
                }
                else
                {
                    throw new ArgumentException(
                        $"scriptName must be amongst predefined scriptNames or a full script provided, script: {scriptName} not found");
                }

                Scripts.ShaCollection[scriptName] = sha;
            }

            var args = new List<string>
            {
                Scripts.ShaCollection[scriptName],
                keys.Length.ToString(),
            };
            args.AddRange(keys);
            args.AddRange(argv);
            return await connection.ExecuteAsync("EVALSHA", args.ToArray());
        }


        public static int? CreateAndEval(this IRedisConnection connection, string scriptName, List<string> keys,
            IEnumerable<string> argv, string fullScript = "")
        {
            if (!Scripts.ShaCollection.ContainsKey(scriptName))
            {
                string sha;
                if (Scripts.ScriptCollection.ContainsKey(scriptName))
                {
                    sha = connection.Execute("SCRIPT", "LOAD", Scripts.ScriptCollection[scriptName]);
                }
                else if (!string.IsNullOrEmpty(fullScript))
                {
                    sha = connection.Execute("SCRIPT", "LOAD", fullScript);
                }
                else
                {
                    throw new ArgumentException(
                        "scriptName must be amongst predefined scriptNames or a full script provided");
                }

                Scripts.ShaCollection[scriptName] = sha;
            }

            var args = new List<string>
            {
                Scripts.ShaCollection[scriptName],
                keys.Count.ToString(),
            };
            args.AddRange(keys);
            args.AddRange(argv);
            return connection.Execute("EVALSHA", args.ToArray());
        }


        public static string Unlink(this IRedisConnection connection, string key) =>
            connection.Execute("UNLINK", key);


        public static async Task<string> UnlinkAsync(this IRedisConnection connection, string key) =>
            await connection.ExecuteAsync("UNLINK", key);


        internal static async Task UnlinkAndSet<T>(this IRedisConnection connection, string? key, T value,
            StorageType storageType)
        {
            _ = value ?? throw new ArgumentNullException(nameof(value));
            if (storageType == StorageType.Json)
            {
                await connection.CreateAndEvalAsync(nameof(Scripts.UnlinkAndSendJson), new[] {key}!,
                    new[] {JsonSerializer.Serialize(value, Options)});
            }
            else
            {
                var hash = value.BuildHashSet();
                var args = new List<string>((hash.Keys.Count * 2) + 1) {hash.Keys.Count.ToString()};
                foreach (var (s, value1) in hash)
                {
                    args.Add(s);
                    args.Add(value1);
                }

                await connection.CreateAndEvalAsync(nameof(Scripts.UnlinkAndSetHash), new[] {key}!, args.ToArray());
            }
        }
    }
}