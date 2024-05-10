using Swag.Access.Data.Interface.Models;

namespace Swag.Access.Data.Interface;

public interface IAbstractDataAccess : IDataAccess
{
    new ICollection<object> GetEstimates();
    new object GetEstimate(int optimistic, int mostLikely, int pessimistic);
    new object AddEstimate(Estimate estimate);
    new object UpdateEstimate(Estimate estimate);
    new object DeleteEstimate(int id);
}

public interface IDataAccess 
{
    Task<IEnumerable<Estimate>> GetEstimates();
    Task<(Estimate Estimate, bool EntryFound)> GetEstimate(int optimistic, int mostLikely, int pessimistic);
    Task<(Estimate? estimate, bool successful)> AddEstimate(Estimate estimate);
    Task<(Estimate? estimate, bool successful)> UpdateEstimate(Estimate estimate);
    Task<bool> DeleteEstimate(int id);
}