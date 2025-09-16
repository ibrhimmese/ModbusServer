using Application.GenericRepositoryFiles.Common.DynamicQueryFilter;
using Newtonsoft.Json;

namespace Application.Interfaces.GenericRepositoryServices.Common.SerializeFilterCommonCache;

public static class SerializeFilterCache
{
    public static string SerializeFilter(Filter filter)
    {
        return JsonConvert.SerializeObject(new
        {
            filter.Field,
            filter.Value,
            filter.Operator,
            filter.Logic,
            Filters = filter.Filters?.Select(SerializeFilterInternal)
        });
    }

    private static object SerializeFilterInternal(Filter filter)
    {
        return new
        {
            filter.Field,
            filter.Value,
            filter.Operator,
            filter.Logic,
            Filters = filter.Filters?.Select(SerializeFilterInternal)
        };
    }
}
