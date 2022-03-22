using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AioCore.Redis.OM.Modeling
{
    public class RedisCollectionStateManager
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        static RedisCollectionStateManager()
        {
            JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
        }


        public RedisCollectionStateManager(DocumentAttribute attr)
        {
            DocumentAttribute = attr;
        }


        public DocumentAttribute DocumentAttribute { get; }


        private IDictionary<string?, object> Snapshot { get; } = default!;


        internal IDictionary<string?, object?> Data { get; set; } = default!;


        internal void Remove(string? key)
        {
            Snapshot.Remove(key);
            Data.Remove(key);
        }


        internal void InsertIntoData(string? key, object? value)
        {
            Data.Remove(key);
            Data.Add(key, value);
        }


        internal void InsertIntoSnapshot(string? key, object? value)
        {
            Snapshot.Remove(key);

            if (DocumentAttribute.StorageType == StorageType.Json)
            {
                var json = JToken.FromObject(value!,
                    Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings
                        {NullValueHandling = NullValueHandling.Ignore}));
                Snapshot.Add(key, json);
            }
            else
            {
                var hash = value.BuildHashSet();
                Snapshot.Add(key, hash);
            }
        }


        internal bool TryDetectDifferencesSingle(string? key, object? value, out IList<IObjectDiff>? differences)
        {
            if (!Snapshot.ContainsKey(key))
            {
                differences = null;
                return false;
            }

            if (DocumentAttribute.StorageType == StorageType.Json)
            {
                var dataJson = JsonSerializer.Serialize(value, JsonSerializerOptions);
                var current = JsonConvert.DeserializeObject<JObject>(dataJson,
                    new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                var snapshot = (JToken) Snapshot[key];
                var diff = FindDiff(current!, snapshot);
                differences = BuildJsonDifference(diff, "$");
            }
            else
            {
                var dataHash = value.BuildHashSet();
                var snapshotHash = (IDictionary<string, string>) Snapshot[key];
                var deletedKeys = snapshotHash.Keys.Except(dataHash.Keys)
                    .Select(x => new KeyValuePair<string, string>(x, string.Empty));
                var modifiedKeys = dataHash.Where(x =>
                    !snapshotHash.Keys.Contains(x.Key) || snapshotHash[x.Key] != x.Value);
                differences = new List<IObjectDiff>
                {
                    new HashDiff(modifiedKeys, deletedKeys.Select(x => x.Key)),
                };
            }

            return true;
        }


        internal IDictionary<string, IList<IObjectDiff>> DetectDifferences()
        {
            var res = new Dictionary<string, IList<IObjectDiff>>();
            if (DocumentAttribute.StorageType == StorageType.Json)
            {
                foreach (var key in Snapshot.Keys)
                {
                    if (Data.ContainsKey(key))
                    {
                        var dataJson = JsonSerializer.Serialize(Data[key], JsonSerializerOptions);
                        var current = JsonConvert.DeserializeObject<JObject>(dataJson,
                            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                        var snapshot = (JToken) Snapshot[key];
                        var diff = FindDiff(current!, snapshot);
                        var diffArgs = BuildJsonDifference(diff, "$");
                        res.Add(key!, diffArgs);
                    }
                    else
                    {
                        res.Add(key!, new List<IObjectDiff> {new JsonDiff("DEL", ".", string.Empty)});
                    }
                }
            }
            else
            {
                foreach (var key in Snapshot.Keys)
                {
                    if (Data.ContainsKey(key))
                    {
                        var dataHash = Data[key] !.BuildHashSet();
                        var snapshotHash = (IDictionary<string, string>) Snapshot[key];
                        var deletedKeys = snapshotHash.Keys.Except(dataHash.Keys)
                            .Select(x => new KeyValuePair<string, string>(x, string.Empty));
                        var modifiedKeys = dataHash.Where(x =>
                            !snapshotHash.ContainsKey(x.Key) || snapshotHash[x.Key] != x.Value);
                        var diff = new List<IObjectDiff>
                        {
                            new HashDiff(modifiedKeys, deletedKeys.Select(x => x.Key)),
                        };
                        res.Add(key!, diff);
                    }
                    else
                    {
                        res.Add(key!, new List<IObjectDiff> {new DelDiff()});
                    }
                }
            }

            return res;
        }

        private static IList<IObjectDiff> BuildJsonDifference(JObject diff, string currentPath)
        {
            var ret = new List<IObjectDiff>();
            if (diff.ContainsKey("+") && diff.ContainsKey("-"))
            {
                if (diff["+"] is JArray arr)
                {
                    var minusArr = (JArray) diff["-"] !;
                    ret.AddRange(arr.Select(item => new JsonDiff("ARRAPPEND", currentPath, item)));

                    ret.AddRange(minusArr.Select(item => new JsonDiff("ARRREM", currentPath, item)));
                }
                else
                {
                    ret.Add(new JsonDiff("SET", currentPath, diff["+"] !));
                }

                return ret;
            }

            if (diff.ContainsKey("+"))
            {
                if (diff["+"] is JArray arr)
                {
                    ret.AddRange(arr.Select(item => new JsonDiff("ARRAPPEND", currentPath, item)));
                }
                else
                {
                    ret.Add(new JsonDiff("SET", currentPath, diff["+"] !));
                }

                return ret;
            }

            if (diff.ContainsKey("-"))
            {
                if (diff["-"] is JArray arr)
                {
                    ret.AddRange(arr.Select(item => new JsonDiff("ARRREM", currentPath, item)));
                }
                else
                {
                    ret.Add(new JsonDiff("DEL", currentPath, string.Empty));
                }

                return ret;
            }

            foreach (var item in diff)
            {
                var val = item.Value as JObject;
                ret.AddRange(BuildJsonDifference(val!, $"{currentPath}.{item.Key}"));
            }

            return ret;
        }


        private static JObject FindDiff(JToken currentObject, JToken snapshotObject)
        {
            var diff = new JObject();
            if (JToken.DeepEquals(currentObject, snapshotObject))
            {
                return diff;
            }

            switch (currentObject.Type)
            {
                case JTokenType.Object:
                {
                    var current = currentObject as JObject;
                    var model = snapshotObject as JObject;
                    if (current == null && model != null)
                    {
                        return new JObject {["-"] = model};
                    }

                    if (current != null && model == null)
                    {
                        return new JObject {["+"] = current};
                    }

                    var addedKeys = current!.Properties()
                        .Select(c => c.Name).Except(model!.Properties().Select(c => c.Name));
                    var removedKeys = model.Properties()
                        .Select(c => c.Name).Except(current.Properties().Select(c => c.Name));
                    var unchangedKeys = current.Properties()
                        .Where(c => JToken.DeepEquals(c.Value, snapshotObject[c.Name])).Select(c => c.Name);
                    var enumerable = addedKeys as string[] ?? addedKeys.ToArray();
                    foreach (var k in enumerable)
                    {
                        diff[k] = new JObject
                        {
                            ["+"] = currentObject[k],
                        };
                    }

                    foreach (var k in removedKeys)
                    {
                        diff[k] = new JObject
                        {
                            ["-"] = snapshotObject[k],
                        };
                    }

                    var potentiallyModifiedKeys = current.Properties().Select(c => c.Name).Except(enumerable)
                        .Except(unchangedKeys);
                    foreach (var k in potentiallyModifiedKeys)
                    {
                        var foundDiff = FindDiff(current[k] !, model[k] !);
                        if (foundDiff.HasValues)
                        {
                            diff[k] = foundDiff;
                        }
                    }
                }

                    break;
                case JTokenType.Array:
                {
                    var current = currentObject as JArray;
                    var model = snapshotObject as JArray;
                    var plus = new JArray(current!.Except(model!, new JTokenEqualityComparer()));
                    var minus = new JArray(model!.Except(current!, new JTokenEqualityComparer()));
                    if (plus.HasValues)
                    {
                        diff["+"] = plus;
                    }

                    if (minus.HasValues)
                    {
                        diff["-"] = minus;
                    }
                }

                    break;
                default:
                    if (currentObject.ToString() != snapshotObject.ToString())
                    {
                        diff["+"] = currentObject;
                        diff["-"] = snapshotObject;
                    }

                    break;
            }

            return diff;
        }
    }
}