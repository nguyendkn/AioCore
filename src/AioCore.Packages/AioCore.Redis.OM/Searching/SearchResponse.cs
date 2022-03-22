namespace AioCore.Redis.OM.Searching
{
    public class SearchResponse
    {
        public SearchResponse(RedisReply val)
        {
            var redisReplies = val.ToArray();
            DocumentCount = redisReplies[0];
            Documents = new Dictionary<string, IDictionary<string, string>>();
            for (var i = 1; i < redisReplies.Length; i += 2)
            {
                var docId = (string) redisReplies[i];
                var documentHash = new Dictionary<string, string>();
                var docArray = redisReplies[i + 1].ToArray();
                for (var j = 0; j < docArray.Length; j += 2)
                {
                    documentHash.Add(docArray[j], docArray[j + 1]);
                }

                Documents.Add(docId, documentHash);
            }
        }


        public long DocumentCount { get; }


        public IDictionary<string, IDictionary<string, string>> Documents { get; }


        public IDictionary<string, T> DocumentsAs<T>()
            where T : notnull
        {
            var dict = new Dictionary<string, T>();
            foreach (var (key, value) in Documents)
            {
                var obj = RedisObjectHandler.FromHashSet<T>(value);
                dict.Add(key, obj);
            }

            return dict;
        }
    }


    public class SearchResponse<T>
        where T : notnull
    {
        public SearchResponse(RedisReply val)
        {
            var type = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (type.IsPrimitive || type == typeof(string))
            {
                var @this = PrimitiveSearchResponse(val);
                Documents = @this.Documents;
                DocumentCount = @this.DocumentCount;
            }
            else if (underlyingType is {IsPrimitive: true})
            {
                var @this = PrimitiveSearchResponse(val);
                Documents = @this.Documents;
                DocumentCount = @this.DocumentCount;
            }
            else
            {
                var vals = val.ToArray();
                DocumentCount = vals[0];
                Documents = new Dictionary<string, T>();
                for (var i = 1; i < vals.Count(); i += 2)
                {
                    var docId = (string) vals[i];
                    var documentHash = new Dictionary<string, string>();
                    var docArray = vals[i + 1].ToArray();
                    for (var j = 0; j < docArray.Length; j += 2)
                    {
                        documentHash.Add(docArray[j], docArray[j + 1]);
                    }

                    var obj = RedisObjectHandler.FromHashSet<T>(documentHash);
                    Documents.Add(docId, obj);
                }
            }
        }

        private SearchResponse()
        {
            DocumentCount = 0;
            Documents = new Dictionary<string, T>();
        }


        public long DocumentCount { get; set; }


        public IDictionary<string, T> Documents { get; }


        public T this[string key] => Documents[key];


        internal T this[int index] => Documents.Values.ElementAt(index);

        private static SearchResponse<T> PrimitiveSearchResponse(RedisReply redisReply)
        {
            var arr = redisReply.ToArray();
            var response = new SearchResponse<T>
            {
                DocumentCount = arr[0]
            };
            for (var i = 1; i < arr.Length; i += 2)
            {
                var docId = (string) arr[i];
                var primitive = arr[i + 1].ToArray().Length > 1
                    ? (T) Convert.ChangeType(arr[i + 1].ToArray()[1], typeof(T))
                    : default;
                response.Documents.Add(docId, primitive!);
            }

            return response;
        }
    }
}