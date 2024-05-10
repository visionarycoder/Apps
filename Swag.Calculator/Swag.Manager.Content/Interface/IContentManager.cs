using Swag.Manager.Content.Interface.Models;

namespace Swag.Manager.Content.Interface;

public interface IContentManager
{
    Task<Estimate> CalculateEstimate(int optimistic, int mostLikely, int pessimistic);
}