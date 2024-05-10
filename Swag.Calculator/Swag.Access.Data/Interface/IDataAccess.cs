using Swag.Access.Data.Interface.Models;

namespace Swag.Access.Data.Interface;

public interface IDataAccess
{
    Task<IEnumerable<Estimate>> GetEstimates();
    Task<(Estimate Estimate, bool EntryFound)> GetEstimate(int optimistic, int mostLikely, int pessimistic);
    Task<(Estimate? estimate, bool successful)> AddEstimate(Estimate estimate);
    Task<(Estimate? estimate, bool successful)> UpdateEstimate(Estimate estimate);
    Task<bool> DeleteEstimate(int id);

}