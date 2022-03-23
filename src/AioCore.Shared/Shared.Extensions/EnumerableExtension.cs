namespace Shared.Extensions;

public static class EnumerableExtension
{
    public static async Task<List<TSource>> ToListAsync<TSource>(this IQueryable<TSource> source)
    {
        if (source == null) throw new ArgumentNullException(typeof(TSource).FullName, "Couldn't not found data source");
        return await Task.FromResult(new List<TSource>(source));
    }
}