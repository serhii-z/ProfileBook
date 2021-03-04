using ProfileBook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Repository
{
    public interface IRepository
    {
        Task<int> InsertAsync<T>(T item) where T : IEntityBase, new();
        Task<int> UpdateAsync<T>(T item) where T : IEntityBase, new();
        Task<int> DeleteAsync<T>(T item) where T : IEntityBase, new();
        Task<T> FindWithQueryAsync<T>(string sql) where T : IEntityBase, new();
        Task<List<T>> QueryAsync<T>(string sql) where T : IEntityBase, new();
    }
}
