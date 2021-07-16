using System;
using System.IO;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Logger
{
    public class LogToDB: ILogger
    {
        private static FileStream file = new("config.json", FileMode.Open);
        private static Config config = JsonSerializer.DeserializeAsync<Config>(file).Result;
        private string connectionString = $"Data Source={config.DataSource};Mode={config.Mode};";

        public SqliteConnection _connection;
        public SqliteCommand _query;

        public LogToDB()
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
           
        public void RecordToLog(string typeevent, string message) 
        {
            _connection.Open();
            _query.CommandText = $"INSERT INTO tab_total_log (type_event, date_time_event, user, message)" +
                    $"VALUES ('{typeevent}', '{DateTime.Now}', '{Environment.UserName}', '{message}')";
            _query.ExecuteNonQuery();
            _connection.Close();
        }

        public void ReadTheLog() 
        {
            _connection.Open();
            var sql = "SELECT * FROM tab_total_log";
            using var result = SelectQuery(sql);

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            var totals = new List<TotalLog>();
            while (result.Read())
            {
                var total = new TotalLog
                {
                    Id = result.GetInt32(0),
                    TypeEvent = result.GetString(1),
                    DateTimeEvent = result.GetString(2),
                    User = result.GetString(3),
                    Message = result.GetString(4)
                };
                totals.Add(total);
            }

            foreach (var total in totals)
                   Console.WriteLine($"{total.Id} | {total.TypeEvent} | {total.DateTimeEvent} | {total.User} | {total.Message}");
            _connection.Close();
        }

        public void ClearLog()
        {
            _connection.Open();
            _query.CommandText = "DELETE FROM tab_total_log";
            _query.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
