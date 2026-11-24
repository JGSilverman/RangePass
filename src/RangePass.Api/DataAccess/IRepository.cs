using System.Linq.Expressions;

namespace RangePass.Api.DataAccess;

public interface IRepository<T> where T : class
{
    Task<bool> DeleteMany(List<T> data);
    Task<bool> DeleteOne(T obj);
    Task<List<T>> GetMany(Expression<Func<T, bool>>? filter = null);
    Task<T?> GetOne(Expression<Func<T, bool>>? filter = null);
    Task<bool> InsertMany(List<T> data);
    Task<T> InsertOne(T data);
    Task<bool> UpdateMany(List<T> data);
    Task<bool> UpdateOne(T data);
}
