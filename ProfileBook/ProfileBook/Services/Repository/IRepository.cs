using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Repository
{
    public interface IRepository
    {
        Task<int> InsertItem<T>(T item);
        Task<int> UpdateItem<T>(T item);
        Task<int> DeleteItem<T>(T item);
        Task<T> FindItem<T>(string sql) where T : new();
        Task<List<T>> ChooseItems<T>(string sql) where T : new();
    }
}
