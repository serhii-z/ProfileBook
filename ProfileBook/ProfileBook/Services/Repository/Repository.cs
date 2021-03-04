using ProfileBook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProfileBook.Servises.Repository
{
    public class Repository : IRepository
    {
        private Lazy<SQLiteAsyncConnection> _database;

        public Repository()
        {
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "profilebook.db");
                var database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<UserModel>();
                database.CreateTableAsync<ProfileModel>();

                return database;
            });
        }

        public Task<int> InsertAsync<T>(T item) where T: IEntityBase, new()
        {
            return _database.Value.InsertAsync(item);
        }

        public Task<int> UpdateAsync<T>(T item) where T : IEntityBase, new()
        {
            return _database.Value.UpdateAsync(item);
        }

        public Task<int> DeleteAsync<T>(T item) where T : IEntityBase, new()
        {
            return _database.Value.DeleteAsync(item);
        }

        public Task<T> FindWithQueryAsync<T>(string sql) where T : IEntityBase, new()
        {
            return _database.Value.FindWithQueryAsync<T>(sql);
        }

        public Task<List<T>> QueryAsync<T>(string sql) where T : IEntityBase, new()
        {
            return _database.Value.QueryAsync<T>(sql);
        }
    }
}
