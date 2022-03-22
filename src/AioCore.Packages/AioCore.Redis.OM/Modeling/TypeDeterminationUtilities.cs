namespace AioCore.Redis.OM.Modeling
{
    internal static class TypeDeterminationUtilities
    {
        private static readonly HashSet<Type> NumericTypes = new()
        {
            typeof(int),
            typeof(double),
            typeof(decimal),
            typeof(long),
            typeof(short),
            typeof(sbyte),
            typeof(byte),
            typeof(ulong),
            typeof(uint),
            typeof(ushort),
            typeof(float),
        };


        internal static bool IsNumeric(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            return NumericTypes.Contains(underlyingType ?? type);
        }


        internal static SearchFieldType GetSearchFieldType(Type type)
        {
            if (IsNumeric(type))
            {
                return SearchFieldType.NUMERIC;
            }

            if (type == typeof(string))
            {
                return SearchFieldType.TAG;
            }

            throw new ArgumentException("Unrecognized Index type, can only index numerics, GeoLoc, or String");
        }
    }
}