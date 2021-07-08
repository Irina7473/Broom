using System;
using System.IO;
using System.Text.Json;
using Microsoft.Data.Sqlite;

namespace connectDB
{
    public class DBLogger
    {
        private static FileStream file = new("config.json", FileMode.Open);
        private static Config config = JsonSerializer.DeserializeAsync<Config>(file).Result;
        private string connectionString = $"Data Source={config.DataSource};Mode={config.Mode};";

        public SqliteConnection _connection;
        public SqliteCommand _query;

        public DBLogger()
        {
            _connection = new SqliteConnection(connectionString);
            _query = new SqliteCommand { Connection = _connection };
        }

        public void Open()
        {
            try
            {
                _connection.Open();
            }
            catch (InvalidOperationException)
            {
                throw new Exception("Ошибка открытия БД");
            }
            catch (SqliteException)
            {
                throw new Exception("Подключаемся к уже открытой БД");
            }
        }

        public void Close()
        {
            _connection.Close();
        }

        public SqliteDataReader SelectQuery(string sql)
        {
            _query.CommandText = sql;
            var result = _query.ExecuteReader();
            return result;
        }        
    }
}