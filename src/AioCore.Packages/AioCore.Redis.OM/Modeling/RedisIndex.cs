namespace AioCore.Redis.OM.Modeling
{
    internal static class RedisIndex
    {
        internal static DocumentAttribute? GetObjectDefinition(this Type type)
        {
            return Attribute.GetCustomAttribute(
                type,
                typeof(DocumentAttribute)) as DocumentAttribute;
        }


        internal static string[] SerializeIndex(this Type type)
        {
            if (Attribute.GetCustomAttribute(type, typeof(DocumentAttribute)) is not DocumentAttribute objAttribute)
            {
                throw new InvalidOperationException($"Type being indexed must be decorated " +
                                                    $"with an RedisObjectDefinitionAttribute, none found on provided type:{type.Name}");
            }

            var args = new List<string>
            {
                string.IsNullOrEmpty(objAttribute.IndexName)
                    ? $"{type.Name.ToLower()}-idx"
                    : objAttribute.IndexName!,
                "ON",
                objAttribute.StorageType.ToString(),
                "PREFIX"
            };

            if (objAttribute.Prefixes.Length > 0)
            {
                args.Add(objAttribute.Prefixes.Length.ToString());
                args.AddRange(objAttribute.Prefixes);
            }
            else
            {
                args.Add("1");
                args.Add($"{type.FullName}:");
            }

            if (!string.IsNullOrEmpty(objAttribute.Filter))
            {
                args.Add("FILTER");
                args.Add(objAttribute.Filter!);
            }

            if (!string.IsNullOrEmpty(objAttribute.Language))
            {
                args.Add("LANGUAGE");
                args.Add(objAttribute.Language!);
            }

            if (!string.IsNullOrEmpty(objAttribute.LanguageField))
            {
                args.Add("LANGUAGE");
                args.Add(objAttribute.LanguageField!);
            }

            args.Add("SCHEMA");
            foreach (var property in type.GetProperties())
            {
                args.AddRange(objAttribute.StorageType == StorageType.Hash
                    ? property.SerializeArgs()
                    : property.SerializeArgsJson());
            }

            return args.ToArray();
        }
    }
}