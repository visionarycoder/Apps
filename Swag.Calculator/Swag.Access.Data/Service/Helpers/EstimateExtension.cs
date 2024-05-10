using Swag.Access.Data.Interface.Models;

namespace Swag.Access.Data.Service.Helpers;

public static class EstimateExtension
{
    
    public static Estimate Convert(this Resource.Data.SwagDb.Models.Estimate source)
    {
        var target = new Estimate
        {
            Id = source.Id,
            Optimistic = source.Optimistic,
            MostLikely = source.MostLikely,
            Pessimistic = source.Pessimistic,
            Calculated = source.Calculated
        };
        return target;
    }

    public static Resource.Data.SwagDb.Models.Estimate Convert(this Estimate source)
    {
        var target = new Resource.Data.SwagDb.Models.Estimate
        {
            Id = source.Id,
            Optimistic = source.Optimistic,
            MostLikely = source.MostLikely,
            Pessimistic = source.Pessimistic,
            Calculated = source.Calculated
        };
        return target;
    }

    public static IEnumerable<Estimate> Convert(this IEnumerable<Resource.Data.SwagDb.Models.Estimate> source)
    {
        return source.Select(i => i.Convert());
    }

    public static IEnumerable<Resource.Data.SwagDb.Models.Estimate> Convert(this IEnumerable<Estimate> source)
    {
        return source.Select(i => i.Convert());
    }

}