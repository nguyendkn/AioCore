using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using AioCore.Redis.OM.Contracts;
using AioCore.Redis.OM.Modeling;

[assembly: InternalsVisibleTo("Redis.OM.POC")]

namespace AioCore.Redis.OM
{
    internal static class RedisObjectHandler
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new();

        private static readonly Dictionary<Type, object?> TypeDefaultCache = new()
        {
            {typeof(string), null},
            {typeof(Guid), default(Guid)},
            {typeof(int), default(int)},
            {typeof(long), default(long)},
            {typeof(uint), default(uint)},
            {typeof(ulong), default(ulong)},
        };

        static RedisObjectHandler()
        {
            JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
        }


        internal static T FromHashSet<T>(IDictionary<string, string> hash)
            where T : notnull
        {
            if (typeof(IRedisHydrated).IsAssignableFrom(typeof(T)))
            {
                var obj = Activator.CreateInstance<T>();
                ((IRedisHydrated) obj).Hydrate(hash);
                return obj;
            }

            var attr = Attribute.GetCustomAttribute(typeof(T), typeof(DocumentAttribute)) as DocumentAttribute;
            string asJson;
            if (attr != null && attr.StorageType == StorageType.Json)
            {
                asJson = hash["$"];
            }
            else
            {
                asJson = SendToJson(hash, typeof(T));
            }

            return JsonSerializer.Deserialize<T>(asJson, JsonSerializerOptions) ??
                   throw new Exception("Deserialization fail");
        }


        internal static object? FromHashSet(IDictionary<string, string> hash, Type type)
        {
            var asJson = SendToJson(hash, type);
            return JsonSerializer.Deserialize(asJson, type);
        }


        private static string GetId(this object obj)
        {
            var type = obj.GetType();
            var idProperty = type.GetProperties()
                .FirstOrDefault(x => Attribute.GetCustomAttribute(x, typeof(RedisIdFieldAttribute)) != null);
            if (idProperty != null)
            {
                return idProperty.GetValue(obj)?.ToString() ?? string.Empty;
            }

            return string.Empty;
        }


        internal static string GetKey(this object obj)
        {
            var type = obj.GetType();
            var documentAttribute = (DocumentAttribute) type.GetCustomAttribute(typeof(DocumentAttribute))!;
            if (documentAttribute == null)
            {
                throw new ArgumentException("Missing Document Attribute on Declaring class");
            }

            var id = obj.GetId();
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Id field is not correctly populated");
            }

            var sb = new StringBuilder();
            if (documentAttribute.Prefixes.Any())
            {
                sb.Append(documentAttribute.Prefixes.First());
                sb.Append(':');
            }
            else
            {
                sb.Append(type.FullName);
                sb.Append(':');
            }

            sb.Append(id);
            return sb.ToString();
        }


        internal static string SetId(this object obj)
        {
            var type = obj.GetType();
            var attr = Attribute.GetCustomAttribute(type, typeof(DocumentAttribute)) as DocumentAttribute;
            var idProperty = type.GetProperties()
                .FirstOrDefault(x => Attribute.GetCustomAttribute(x, typeof(RedisIdFieldAttribute)) != null);
            if (attr == null)
            {
                throw new MissingMemberException("Missing Document Attribute decoration");
            }

            var id = attr.IdGenerationStrategy.GenerateId();
            if (idProperty == null)
                return string.IsNullOrEmpty(attr.Prefixes.FirstOrDefault())
                    ? $"{type.FullName}:{id}"
                    : $"{attr.Prefixes.First()}:{id}";
            var idPropertyType = idProperty.PropertyType;
            var supportedIdPropertyTypes = new[] {typeof(string), typeof(Guid)};
            if (!supportedIdPropertyTypes.Contains(idPropertyType) && !idPropertyType.IsValueType)
            {
                throw new InvalidOperationException(
                    "Software Defined Ids on objects must either be a string, Guid, or some other value type.");
            }

            var currId = idProperty.GetValue(obj);

            if (!TypeDefaultCache.ContainsKey(idPropertyType))
            {
                TypeDefaultCache.Add(idPropertyType, Activator.CreateInstance(idPropertyType));
            }

            if (currId?.ToString() != TypeDefaultCache[idPropertyType]?.ToString())
            {
                id = idProperty.GetValue(obj)?.ToString();
            }
            else
            {
                if (idPropertyType == typeof(Guid))
                {
                    idProperty.SetValue(obj, Guid.Parse(id));
                }
                else
                {
                    idProperty.SetValue(obj, id);
                }
            }

            return string.IsNullOrEmpty(attr.Prefixes.FirstOrDefault())
                ? $"{type.FullName}:{id}"
                : $"{attr.Prefixes.First()}:{id}";
        }


        private static void ExtractPropertyName(ICustomAttributeProvider property, ref string propertyName)
        {
            var fieldAttr = property.GetCustomAttributes(typeof(RedisFieldAttribute), true);
            if (!fieldAttr.Any()) return;
            var rfa = (RedisFieldAttribute) fieldAttr.First();
            if (!string.IsNullOrEmpty(rfa.PropertyName))
            {
                propertyName = rfa.PropertyName;
            }
        }


        internal static T ToObject<T>(this RedisReply val)
            where T : notnull
        {
            var hash = new Dictionary<string, string>();
            var redisReplies = val.ToArray();
            for (var i = 0; i < redisReplies.Length; i += 2)
            {
                hash.Add(redisReplies[i], redisReplies[i + 1]);
            }

            return FromHashSet<T>(hash);
        }


        internal static IDictionary<string, string> BuildHashSet(this object? obj)
        {
            if (obj is IRedisHydrated redisHydrated)
            {
                return redisHydrated.BuildHashSet();
            }

            var properties = obj!
                .GetType()
                .GetProperties()
                .Where(x => x.GetValue(obj) != null);
            var hash = new Dictionary<string, string>();
            foreach (var property in properties)
            {
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                var propertyName = property.Name;
                ExtractPropertyName(property, ref propertyName);
                if (type.IsPrimitive || type == typeof(decimal) || type == typeof(string) || type == typeof(Guid))
                {
                    var val = property.GetValue(obj);
                    if (val != null)
                    {
                        hash.Add(propertyName, val.ToString()!);
                    }
                }
                else if (type == typeof(DateTimeOffset))
                {
                    var val = (DateTimeOffset) (property.GetValue(obj) ?? default!);
                    hash.Add(propertyName, val.ToString("O"));
                }
                else if (type == typeof(DateTime) || type == typeof(DateTime?))
                {
                    var val = (DateTime) (property.GetValue(obj) ?? default!);
                    if (val != default)
                    {
                        hash.Add(propertyName, new DateTimeOffset(val).ToUnixTimeMilliseconds().ToString());
                    }
                }
                else if (type.GetInterfaces()
                         .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    var e = (IEnumerable<object>) property.GetValue(obj)!;
                    var i = 0;
                    foreach (var v in e)
                    {
                        var innerType = v.GetType();
                        if (innerType.IsPrimitive || innerType == typeof(decimal) || innerType == typeof(string))
                        {
                            hash.Add($"{propertyName}[{i}]", v.ToString()!);
                        }
                        else
                        {
                            var subHash = v.BuildHashSet();
                            foreach (var (key, value) in subHash)
                            {
                                hash.Add($"{propertyName}.[{i}].{key}", value);
                            }
                        }

                        i++;
                    }
                }
                else
                {
                    var subHash = property.GetValue(obj)?.BuildHashSet();
                    if (subHash == null) continue;
                    foreach (var (key, value) in subHash)
                    {
                        hash.Add($"{propertyName}.{key}", value);
                    }
                }
            }

            return hash;
        }

        private static string SendToJson(IDictionary<string, string> hash, Type t)
        {
            var properties = t.GetProperties();
            var ret = "{";
            foreach (var property in properties)
            {
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var propertyName = property.Name;
                ExtractPropertyName(property, ref propertyName);
                if (!hash.Any(x => x.Key.StartsWith(propertyName)))
                {
                    continue;
                }

                if (type == typeof(bool) || type == typeof(bool?))
                {
                    ret += $"\"{propertyName}\":{hash[propertyName].ToLower()},";
                }
                else if (type.IsPrimitive || type == typeof(decimal))
                {
                    ret += $"\"{propertyName}\":{hash[propertyName]},";
                }
                else if (type == typeof(string) || type == typeof(DateTime) ||
                         type == typeof(DateTime?) || type == typeof(DateTimeOffset))
                {
                    ret += $"\"{propertyName}\":\"{hash[propertyName]}\",";
                }
                else if (type.GetInterfaces()
                         .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    var entries = hash.Where(x => x.Key.StartsWith($"{propertyName}["))
                        .ToDictionary(x => x.Key, x => x.Value);
                    var innerType = type.GetGenericArguments().SingleOrDefault();
                    if (innerType == null)
                    {
                        throw new ArgumentException(
                            "Only a single Generic type is supported on enums for the Hash type");
                    }

                    if (!entries.Any()) continue;
                    {
                        ret += $"\"{propertyName}\":[";
                        for (var i = 0; i < entries.Count; i++)
                        {
                            if (innerType == typeof(bool) || innerType == typeof(bool?))
                            {
                                var val = entries[$"{propertyName}[{i}]"];
                                ret += $"{val.ToLower()},";
                            }
                            else if (innerType.IsPrimitive || innerType == typeof(decimal))
                            {
                                var val = entries[$"{propertyName}[{i}]"];
                                ret += $"{val},";
                            }
                            else if (innerType == typeof(string))
                            {
                                var val = entries[$"{propertyName}[{i}]"];
                                ret += $"\"{val}\",";
                            }
                            else
                            {
                                var dictionary = entries.Where(x => x.Key.StartsWith($"{propertyName}[{i}]"))
                                    .Select(x => new KeyValuePair<string, string>(
                                        x.Key[$"{propertyName}[{i}]".Length..], x.Value))
                                    .ToDictionary(x => x.Key, x => x.Value);
                                if (!dictionary.Any()) continue;
                                ret += SendToJson(entries, innerType);
                                ret += ",";
                            }
                        }

                        ret = ret.TrimEnd(',');
                        ret += "],";
                    }
                }
                else
                {
                    var entries = hash.Where(x => x.Key.StartsWith($"{propertyName}."))
                        .Select(x =>
                            new KeyValuePair<string, string>(x.Key.Substring($"{propertyName}.".Length), x.Value))
                        .ToDictionary(x => x.Key, x => x.Value);
                    if (!entries.Any()) continue;
                    ret += $"\"{propertyName}\":";
                    ret += SendToJson(entries, type);
                    ret += ",";
                }
            }

            ret = ret.TrimEnd(',');
            ret += "}";
            return ret;
        }
    }
}