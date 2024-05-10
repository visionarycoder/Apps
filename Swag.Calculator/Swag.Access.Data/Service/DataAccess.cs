using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swag.Access.Data.Interface;
using Swag.Access.Data.Interface.Models;
using Swag.Access.Data.Service.Helpers;
using Swag.Resource.Data.SwagDb;

namespace Swag.Access.Data.Service;

public class DataAccess(ILogger<DataAccess> logger, DbContextOptions<SwagContext> options) : IDataAccess 
{

    public async Task<IEnumerable<Estimate>> GetEstimates()
    {

        try
        {
            var db = new SwagContext(options);
            var query = db.Estimates.AsQueryable();
            query = query.OrderBy(i => i.Id);
            var dbObjs = await query.ToListAsync();
            var dtos = dbObjs.Convert();
            return dtos;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in {Method}", nameof(GetEstimates));
            return Array.Empty<Estimate>();
        }

    }

    public async Task<(Estimate? estimate, bool successful)> GetEstimate(int id)
    {

        try
        {
            var db = new SwagContext(options);
            var query = db.Estimates.AsQueryable();
            var dbObj = await query.SingleOrDefaultAsync(i => i.Id == id);
            if (dbObj is null)
            {
                return (null, false);
            }

            var dto = dbObj.Convert();
            return (dto, true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in {Method}", nameof(GetEstimate));
            return (null, false);
        }
    }

    public async Task<(Estimate Estimate, bool EntryFound)> GetEstimate(int optimistic, int mostLikely, int pessimistic)
    {
        try
        {
            var db = new SwagContext(options);
            var query = db.Estimates.AsQueryable();
            var dbObj = await query.SingleOrDefaultAsync(i =>
                i.Optimistic == optimistic && i.MostLikely == mostLikely && i.Pessimistic == pessimistic);
            if (dbObj is null)
            {
                return (null, false);
            }

            var dto = dbObj.Convert();
            return (dto, true);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in {Method}", nameof(GetEstimate));
            return (null, false);
        }
    }

    public async Task<(Estimate? estimate, bool successful)> AddEstimate(Estimate estimate)
    {
        try
        {
            var db = new SwagContext(options);
            var dbo = estimate.Convert();
            var alreadyExists = await db.Estimates.AnyAsync(i =>
                i.Optimistic == dbo.Optimistic && i.MostLikely == dbo.MostLikely && i.Pessimistic == dbo.Pessimistic);
            if (alreadyExists)
            {
                return (estimate, false);
            }

            var entityEntry = db.Estimates.Add(dbo);
            var count = await db.SaveChangesAsync();
            if (count == 0)
            {
                return (null, false);
            }

            var dto = entityEntry.Entity.Convert();
            return (dto, true);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in {Method}", nameof(AddEstimate));
            return (null, false);
        }
    }

    public async Task<(Estimate? estimate, bool successful)> UpdateEstimate(Estimate estimate)
    {
        try
        {
            var db = new SwagContext(options);
            var dbo = estimate.Convert();
            var canUpdate = await db.Estimates.AnyAsync(i =>
                i.Optimistic == dbo.Optimistic && i.MostLikely == dbo.MostLikely && i.Pessimistic == dbo.Pessimistic);
            if (!canUpdate)
            {
                return await AddEstimate(estimate);
            }

            db.Estimates.Update(dbo);
            var count = await db.SaveChangesAsync();
            if (count == 0)
            {
                return (null, false);
            }

            var dto = dbo.Convert();
            return (dto, true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in {Method}", nameof(UpdateEstimate));
            return (null, false);
        }
    }

    public async Task<bool> DeleteEstimate(int id)
    {
        try
        {
            var db = new SwagContext(options);
            db.Estimates.Remove(new Resource.Data.SwagDb.Models.Estimate { Id = id });
            var count = await db.SaveChangesAsync();
            return count != 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in {Method}", nameof(DeleteEstimate));
            return false;
        }
    }

}