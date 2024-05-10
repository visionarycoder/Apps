
using Swag.Client.ConsoleApp.Interface.Models;

namespace Swag.Client.ConsoleApp.Service.Helpers;

public static class EstimateExtension
{
    
    public static Estimate Convert(this Manager.Content.Interface.Models.Estimate source)
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

    public static Manager.Content.Interface.Models.Estimate Convert(this Estimate source)
    {
        var target = new Manager.Content.Interface.Models.Estimate
        {
            Id = source.Id,
            Optimistic = source.Optimistic,
            MostLikely = source.MostLikely,
            Pessimistic = source.Pessimistic,
            Calculated = source.Calculated
        };
        return target;
    }

    public static IEnumerable<Estimate> Convert(this IEnumerable<Manager.Content.Interface.Models.Estimate> source)
    {
        return source.Select(i => i.Convert());
    }

    public static IEnumerable<Manager.Content.Interface.Models.Estimate> Convert(this IEnumerable<Estimate> source)
    {
        return source.Select(i => i.Convert());
    }

}