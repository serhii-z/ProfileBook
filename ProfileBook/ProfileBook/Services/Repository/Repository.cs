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

        public async Task<int> SaveItem<T>(T item)
        {
            return await _database.InsertAsync(item).ConfigureAwait(false);
        }

        public async Task<int> UpdateItem<T>(T item)
        {
            return await _database.UpdateAsync(item).ConfigureAwait(false);
        }

        public async Task<int> DeleteItem<T>(T item)
        {
            return await _database.DeleteAsync(item).ConfigureAwait(false);
        }

        public async Task<T> FindItem<T>(string sql) where T : new()
        {
            return await _database.FindWithQueryAsync<T>(sql).ConfigureAwait(false);
        }

        public async Task<List<T>> GetListItems<T>(string sql) where T : new()
        {
            return await _database.QueryAsync<T>(sql).ConfigureAwait(false);
        }
    }
}
