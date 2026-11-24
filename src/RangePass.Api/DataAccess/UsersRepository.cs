using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RangePass.Api.Data;

namespace RangePass.Api.DataAccess;

public sealed class UsersRepository : IRepository<ApplicationUser>
{
    private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

    public UsersRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public Task<bool> DeleteMany(List<ApplicationUser> data)
        => DeleteManyAsync(data);

    public async Task<bool> DeleteManyAsync(List<ApplicationUser> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Users.RemoveRange(data);
        return await context.SaveChangesAsync() >= 1;
    }

    public Task<bool> DeleteOne(ApplicationUser obj)
        => DeleteOneAsync(obj);

    public async Task<bool> DeleteOneAsync(ApplicationUser obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Users.Remove(obj);
        return await context.SaveChangesAsync() >= 1;
    }

    public Task<List<ApplicationUser>> GetMany(Expression<Func<ApplicationUser, bool>>? filter = null)
        => GetManyAsync(filter);

    public async Task<List<ApplicationUser>> GetManyAsync(Expression<Func<ApplicationUser, bool>>? filter = null)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        IQueryable<ApplicationUser> query = context.Users.AsNoTracking();

        if (filter is not null)
            query = query.Where(filter);

        return await query.ToListAsync();
    }

    public Task<ApplicationUser?> GetOne(Expression<Func<ApplicationUser, bool>>? filter = null)
        => GetOneAsync(filter);

    public async Task<ApplicationUser?> GetOneAsync(Expression<Func<ApplicationUser, bool>>? filter = null)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        IQueryable<ApplicationUser> query = context.Users.AsNoTracking();

        if (filter is not null)
            query = query.Where(filter);

        return await query.FirstOrDefaultAsync();
    }

    public Task<bool> InsertMany(List<ApplicationUser> data)
        => InsertManyAsync(data);

    public async Task<bool> InsertManyAsync(List<ApplicationUser> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        await using var context = await _contextFactory.CreateDbContextAsync();
        await context.Users.AddRangeAsync(data);
        return await context.SaveChangesAsync() >= 1;
    }

    public Task<ApplicationUser> InsertOne(ApplicationUser data)
        => InsertOneAsync(data);

    public async Task<ApplicationUser> InsertOneAsync(ApplicationUser data)
    {
        ArgumentNullException.ThrowIfNull(data);

        await using var context = await _contextFactory.CreateDbContextAsync();
        await context.Users.AddAsync(data);
        await context.SaveChangesAsync();
        return data;
    }

    public Task<bool> UpdateMany(List<ApplicationUser> data)
        => UpdateManyAsync(data);

    public async Task<bool> UpdateManyAsync(List<ApplicationUser> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Users.UpdateRange(data);
        return await context.SaveChangesAsync() >= 1;
    }

    public Task<bool> UpdateOne(ApplicationUser data)
        => UpdateOneAsync(data);

    public async Task<bool> UpdateOneAsync(ApplicationUser updatedData)
    {
        ArgumentNullException.ThrowIfNull(updatedData);

        await using var context = await _contextFactory.CreateDbContextAsync();
        var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Id == updatedData.Id);

        if (existingUser is null)
            throw new InvalidOperationException("ApplicationUser does not exist.");

        context.Entry(existingUser).CurrentValues.SetValues(updatedData);

        if (!context.ChangeTracker.HasChanges())
            return true;

        return await context.SaveChangesAsync() >= 1;
    }
}
