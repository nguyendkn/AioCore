using System.ComponentModel;
using AioCore.Shared.ValueObjects;

namespace AioCore.Shared.Extensions;

public static class EnumExtension
{
    public static List<Option<int>> ToEnums<TEnum>(this Type enumType) where TEnum : Enum
    {
        var values = Enum.GetValues(typeof(TEnum)).Cast<int>().ToArray();
        return values.Select(x => new Option<int>
        {
            Name = GetDescription<TEnum>(x, enumType),
            Value = x,
        }).ToList();
    }

    private static string GetDescription<T>(int e, Type type)
    {
        var values = Enum.GetValues(type);

        foreach (int val in values)
        {
            if (val != e) continue;
            var memInfo = type.GetMember(type.GetEnumName(val)!);

            if (memInfo[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                return descriptionAttribute.Description;
        }

        return string.Empty;
    }
}