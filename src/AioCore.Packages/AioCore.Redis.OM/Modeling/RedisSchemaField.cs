﻿using System.Globalization;
using System.Reflection;

namespace AioCore.Redis.OM.Modeling
{
    internal static class RedisSchemaField
    {
        internal static string[] SerializeArgsJson(this PropertyInfo info, int remainingDepth = -1,
            string pathPrefix = "$.", string aliasPrefix = "")
        {
            var attributes = info.GetCustomAttributes()
                .Where(x => x is SearchFieldAttribute)
                .Cast<SearchFieldAttribute>()
                .ToArray();

            if (!attributes.Any())
            {
                return Array.Empty<string>();
            }

            var ret = new List<string>();
            foreach (var attr in attributes)
            {
                if (attr.JsonPath != null)
                {
                    ret.AddRange(SerializeIndexFromJsonPaths(info, attr));
                }
                else
                {
                    var innerType = Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType;

                    if (!TypeDeterminationUtilities.IsNumeric(innerType)
                        && innerType != typeof(string)
                        && !IsTypeIndexableArray(innerType))
                    {
                        var cascadeDepth = remainingDepth == -1 ? attr.CascadeDepth : remainingDepth;
                        if (cascadeDepth <= 0) continue;
                        foreach (var property in info.PropertyType.GetProperties())
                        {
                            ret.AddRange(property.SerializeArgsJson(cascadeDepth - 1, $"{pathPrefix}{info.Name}.",
                                string.IsNullOrEmpty(aliasPrefix)
                                    ? $"{info.Name}_"
                                    : $"{aliasPrefix}{info.Name}_"));
                        }
                    }
                    else
                    {
                        var pathPostFix = IsTypeIndexableArray(innerType) ? "[*]" : string.Empty;
                        ret.Add(!string.IsNullOrEmpty(attr.PropertyName)
                            ? $"{pathPrefix}{attr.PropertyName}{pathPrefix}"
                            : $"{pathPrefix}{info.Name}{pathPostFix}");
                        ret.Add("AS");
                        ret.Add(!string.IsNullOrEmpty(attr.PropertyName)
                            ? $"{aliasPrefix}{attr.PropertyName}"
                            : $"{aliasPrefix}{info.Name}");
                        ret.AddRange(CommonSerialization(attr, innerType));
                    }
                }
            }

            return ret.ToArray();
        }


        internal static string[] SerializeArgs(this PropertyInfo info)
        {
            var attr = Attribute.GetCustomAttribute(info, typeof(SearchFieldAttribute)) as SearchFieldAttribute;
            if (attr == null)
            {
                return Array.Empty<string>();
            }

            var ret = new List<string> {!string.IsNullOrEmpty(attr.PropertyName) ? attr.PropertyName : info.Name};
            var innerType = Nullable.GetUnderlyingType(info.PropertyType);
            ret.AddRange(CommonSerialization(attr, innerType ?? info.PropertyType));
            return ret.ToArray();
        }

        private static bool IsTypeIndexableArray(Type t) => t == typeof(string[]) || t == typeof(bool[]) ||
                                                            t == typeof(List<string>) || t == typeof(List<bool>);

        private static IEnumerable<string> SerializeIndexFromJsonPaths(PropertyInfo parentInfo,
            SearchFieldAttribute attribute, string prefix = "$.")
        {
            var indexArgs = new List<string>();
            var path = attribute.JsonPath;
            var propertyNames = path!.Split('.').Skip(1).ToArray();
            var type = parentInfo.PropertyType;
            foreach (var name in propertyNames)
            {
                var childProperty = type.GetProperty(name);
                if (childProperty == null)
                {
                    throw new RedisIndexingException($"{path} not found in {parentInfo.Name} object graph.");
                }

                type = childProperty.PropertyType;
            }

            indexArgs.Add($"{prefix}{parentInfo.Name}{path.Substring(1)}");
            indexArgs.Add("AS");
            indexArgs.Add($"{parentInfo.Name}_{string.Join("_", propertyNames)}");
            var underlyingType = Nullable.GetUnderlyingType(type);
            indexArgs.AddRange(CommonSerialization(attribute, underlyingType ?? type));
            return indexArgs;
        }

        private static string GetSearchFieldType(SearchFieldType typeEnum, Type declaredType)
        {
            if (typeEnum != SearchFieldType.INDEXED)
            {
                return typeEnum.ToString();
            }

            return TypeDeterminationUtilities.IsNumeric(declaredType) ? "NUMERIC" : "TAG";
        }

        private static IEnumerable<string> CommonSerialization(SearchFieldAttribute attr, Type declaredType)
        {
            var searchFieldType = GetSearchFieldType(attr.SearchFieldType, declaredType);
            var ret = new List<string> {searchFieldType};
            if (attr is SearchableAttribute text)
            {
                if (text.NoStem)
                {
                    ret.Add("NOSTEM");
                }

                if (!string.IsNullOrEmpty(text.PhoneticMatcher))
                {
                    ret.Add("PHONETIC");
                    ret.Add(text.PhoneticMatcher);
                }

                if (Math.Abs(text.Weight - 1) > .0001)
                {
                    ret.Add("WEIGHT");
                    ret.Add(text.Weight.ToString(CultureInfo.InvariantCulture));
                }
            }

            if (searchFieldType == "TAG" && attr is IndexedAttribute tag)
            {
                if (tag.Separator != ',')
                {
                    ret.Add("SEPARATOR");
                    ret.Add(tag.Separator.ToString());
                }

                if (tag.CaseSensitive)
                {
                    ret.Add("CASESENSITIVE");
                }
            }

            if (attr.Sortable || attr.Aggregatable)
            {
                ret.Add("SORTABLE");
            }

            if (attr.Sortable && !attr.Normalize)
            {
                ret.Add("UNF");
            }

            return ret.ToArray();
        }
    }
}