using System.Collections;
using System.Dynamic;

namespace AioCore.Blazor.Template;

public class AnonymousTypeWrapper : DynamicObject
{
    private readonly object? _model;

    public AnonymousTypeWrapper(object? model)
    {
        _model = model;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        var propertyInfo = _model?.GetType().GetProperty(binder.Name);

        if (propertyInfo == null)
        {
            result = null;
            return false;
        }

        result = propertyInfo.GetValue(_model, null);

        if (result == null)
        {
            return true;
        }

        if (result.IsAnonymous())
        {
            result = new AnonymousTypeWrapper(result);
        }

        if (result is IDictionary dictionary)
        {
            var keys = new List<object>();

            foreach (var key in dictionary.Keys)
            {
                keys.Add(key);
            }

            foreach (var key in keys)
            {
                if (dictionary[key].IsAnonymous())
                {
                    dictionary[key] = new AnonymousTypeWrapper(dictionary[key]);
                }
            }
        }
        else if (result is IEnumerable enumerable and not string)
        {
            result = enumerable.Cast<object>()
                .Select(e => e.IsAnonymous() ? new AnonymousTypeWrapper(e) : e)
                .ToList();
        }


        return true;
    }
}