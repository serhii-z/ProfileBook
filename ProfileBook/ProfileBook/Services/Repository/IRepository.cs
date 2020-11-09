using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Repository
{
    public interface IRepository
    {
        Task<int> SaveItem<T>(T item);
        Task<int> UpdateItem<T>(T item);
        Task<int> DeleteItemAsync<T>(T item);
        Task<List<T>> GetListItems<T>(string sql) where T : new();
    }
}
