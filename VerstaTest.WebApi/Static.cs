using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace VerstaTest.WebApi;

public static class Static
{
    public static async Task<string> GetRawBodyAsync(this HttpRequest request, Encoding encoding = null)
    {
        if (!request.Body.CanSeek)
        {
            // We only do this if the stream isn't *already* seekable,
            // as EnableBuffering will create a new stream instance
            // each time it's called
            request.EnableBuffering();
        }

        request.Body.Position = 0;

        StreamReader reader = new(request.Body, encoding ?? Encoding.UTF8);

        string body = await reader.ReadToEndAsync().ConfigureAwait(false);

        request.Body.Position = 0;

        return body;
    }

    public static IQueryable<TSource> OrderBy<TSource>(
       this IQueryable<TSource> query, string? propertyName, string? sortOrderAsString)
    {
        if (propertyName is null || propertyName.Equals("undefined", StringComparison.OrdinalIgnoreCase) || sortOrderAsString is null || sortOrderAsString.Equals("undefined", StringComparison.OrdinalIgnoreCase))
        {
            return query;
        }

        Type entityType = typeof(TSource);

        string[] propertyChain = propertyName.Split('.');

        PropertyInfo? propertyInfo = GetPropertyByPath<TSource>(propertyChain);

        if (propertyInfo is null)
        {
            return query;
        }

        SortOrder? sortOrder = sortOrderAsString switch
        {
            "desc" => SortOrder.Descending,
            "asc" => SortOrder.Ascending,
            _ => null
        };

        if (sortOrder is null)
        {
            return query;
        }

        string sortMethodName = sortOrder switch
        {
            SortOrder.Ascending => "OrderBy",
            SortOrder.Descending => "OrderByDescending"
        };

        ParameterExpression arg = Expression.Parameter(entityType, "x");
        MemberExpression property = Expression.Property(arg, propertyChain[0]);
        if (propertyChain.Length > 1)
        {
            foreach (var chainLink in propertyChain.Skip(1))
            {
                property = Expression.Property(property, chainLink);
            }
        }

        LambdaExpression selector = Expression.Lambda(property, [arg]);

        Type enumarableType = typeof(Queryable);
        MethodInfo method = enumarableType.GetMethods()
             .Where(m => m.Name == sortMethodName && m.IsGenericMethodDefinition)
             .Where(m =>
             {
                 List<ParameterInfo> parameters = m.GetParameters().ToList();

                 return parameters.Count == 2;
             }).Single();

        MethodInfo genericMethod = method
             .MakeGenericMethod(entityType, propertyInfo.PropertyType);

        IOrderedQueryable<TSource>? newQuery = (IOrderedQueryable<TSource>)genericMethod
             .Invoke(genericMethod, [query, selector]);

        if (newQuery is null)
        {
            return query;
        }

        return newQuery;
    }

    public static IQueryable<TSource> Where<TSource>(
       this IQueryable<TSource> query, Dictionary<string, string> dict)
    {
        if (dict is null || dict.Count == 0)
        {
            return query;
        }

        foreach ((string key, string val) in dict)
        {
            try
            {
                query = query.Where(key, val);
            }
            catch { }
        }

        return query;
    }

    private static PropertyInfo? GetPropertyByPath<TSource>(string[] propertyChain)
    {
        Type entityType = typeof(TSource);

        PropertyInfo? propertyInfo = entityType.GetProperty(propertyChain[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (propertyChain.Length > 1)
        {
            foreach (string? chainLink in propertyChain.Skip(1))
            {
                propertyInfo = propertyInfo.PropertyType.GetProperty(chainLink, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }
        }

        return propertyInfo;
    }

    public static IQueryable<TSource> Where<TSource>(
       this IQueryable<TSource> query, string propertyName, string value)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            return query;
        }

        Type entityType = typeof(TSource);

        string[] propertyChain = propertyName.Split('.');

        PropertyInfo? propertyInfo = GetPropertyByPath<TSource>(propertyChain);

        if (propertyInfo is null)
        {
            return query;
        }

        ParameterExpression arg = Expression.Parameter(entityType, "x");
        MemberExpression property = Expression.Property(arg, propertyChain[0]);
        if (propertyChain.Length > 1)
        {
            foreach (string? chainLink in propertyChain.Skip(1))
            {
                property = Expression.Property(property, chainLink);
            }
        }

        MethodInfo containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);
        ConstantExpression someValue = Expression.Constant(value, typeof(string));

        MethodCallExpression containsMethodExp;

        if (propertyInfo.PropertyType == typeof(string))
        {
            containsMethodExp = Expression.Call(property, containsMethod, someValue);
        }
        else
        {
            MethodInfo toStringMethod = propertyInfo.PropertyType.GetMethods(BindingFlags.Public | BindingFlags.Instance).First(p => p.Name == "ToString");

            MethodCallExpression propertyToString = Expression.Call(property, toStringMethod);

            containsMethodExp = Expression.Call(propertyToString, containsMethod, someValue);
        }

        LambdaExpression selector = Expression.Lambda(containsMethodExp, [arg]);

        Type enumarableType = typeof(Queryable);
        MethodInfo method = enumarableType.GetMethods()
             .Where(m => m.Name == "Where" && m.IsGenericMethodDefinition)
             .First();

        MethodInfo genericMethod = method.MakeGenericMethod(entityType);

        IOrderedQueryable<TSource>? newQuery = (IOrderedQueryable<TSource>)genericMethod
             .Invoke(genericMethod, [query, selector]);

        if (newQuery is null)
        {
            return query;
        }

        return newQuery;
    }
}
