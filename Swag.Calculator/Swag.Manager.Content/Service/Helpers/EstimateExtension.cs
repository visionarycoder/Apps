using Swag.Manager.Content.Interface.Models;

namespace Swag.Manager.Content.Service.Helpers;

public static class EstimateExtension
{
    
    public static Estimate Convert(this Access.Data.Interface.Models.Estimate source)
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

}