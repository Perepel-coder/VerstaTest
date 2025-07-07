using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Reflection;

namespace VerstaTest.WebApi.Controllers;


[ApiController]
public class BaseController : Controller
{
    protected async Task<IActionResult> GetJson<TSource, TDestination>(IQueryable<TSource> dbSet, Func<TSource, TDestination> mapFunction)
        where TSource : class
    {
        string rawRequestBody = await Request.GetRawBodyAsync();

        Dictionary<string, string> dict = rawRequestBody.Split("&").Select(i => i.Split("=", StringSplitOptions.RemoveEmptyEntries)).Where(i => i.Length == 2).ToDictionary(i => i[0], i => i[1]);

        int pageIndex = 0;
        if (dict.TryGetValue("pageIndex", out string? pageIndexAsString))
        {
            pageIndex = int.Parse(pageIndexAsString);
        }

        int pageSize = 0;
        if (dict.TryGetValue("pageSize", out string? pageSizeAsString))
        {
            pageSize = int.Parse(pageSizeAsString);
        }

        dict.TryGetValue("sortField", out string? sortField);
        if (!string.IsNullOrEmpty(sortField))
        {
            PropertyInfo? sortProp = typeof(TDestination).GetProperty(sortField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            sortField = sortProp?.GetCustomAttribute<PathAttribute>()?.Path ?? sortField;
        }


        dict.TryGetValue("sortOrder", out string? sortOrder);

        string[] fixedFields = new string[] { "pageIndex", "pageSize", "sortField", "sortOrder" };

        string[] filterKeys = dict.Keys.Except(fixedFields).ToArray();


        int startIndex = (pageIndex - 1) * pageSize;
        Dictionary<string, string> newDict = filterKeys.Select(i => (str: i, prop: typeof(TDestination).GetProperty(i, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance))).ToDictionary(i => i.prop.GetCustomAttribute<PathAttribute>()?.Path ?? i.str, i => Uri.UnescapeDataString(dict[i.str]));

        List<TDestination> list = new();

        int totalItemCount = dbSet.Count();

        foreach (TSource? item in dbSet
            .Where(newDict)
            .OrderBy(sortField, sortOrder)
            .Skip(startIndex)
            .Take(pageSize)
            )
        {
            list.Add(mapFunction(item));
        }

        return Json(new { itemsCount = totalItemCount, data = list });
    }



}