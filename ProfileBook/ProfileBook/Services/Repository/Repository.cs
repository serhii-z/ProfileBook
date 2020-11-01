using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Repository
{
    public class Repository : IRepository
    {
        private SQLiteAsyncConnection _database;
        private const string DATABASE_NAME = "profileBook.db";
        private string DatabasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
            DATABASE_NAME);

        public Repository()
        {
            _database = new SQLiteAsyncConnection(DatabasePath);
        }

        public async Task<List<T>> GetListItems<T>(string sql) where T : new()
        {
            return await _database.QueryAsync<T>(sql).ConfigureAwait(false);
        }

        public Task<T> GetItem<T>(int id) where T : new()
        {
            return _database.GetAsync<T>(id);
        }

        public async Task<int> SaveItem<T>(T item)
        {
            try
            {
                return await _database.InsertAsync(item);
            }
            catch
            {
                return 0;
            }
        }

        public async void UpdateItem<T>(T item)
        {
            await _database.UpdateAsync(item);
        }

        public Task<int> DeleteItemAsync<T>(T item)
        {
            return _database.DeleteAsync(item);
        }
    }
}
