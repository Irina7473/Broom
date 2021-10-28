using System;
using System.IO;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Logger
{
    public class LogToDB: ILogger
    {
        private static FileStream file;
        private static Config config;
        private string connectionString;
        private SqliteConnection _connection;
        private SqliteCommand _query;

        public LogToDB()
        {
            file = new("configLog.json", FileMode.Open);
            config = JsonSerializer.DeserializeAsync<Config>(file).Result;
            connectionString = $"Data Source={config.DataSource};Mode={config.Mode};";
            _connection = new SqliteConnection(connectionString);
            _query = new SqliteCommand { Connection = _connection };
        }

        public LogToDB(string patch)
        {
            connectionString = $"Data Source={patch};Mode=ReadWrite;";            
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
                throw new Exception("Ошибка открытия базы данных");
            }
            catch (SqliteException)
            {
                throw new Exception("Подключаемся к уже открытой базе данных");
            }
            catch (Exception)
            {
                throw new Exception("Путь к базе данных не найден");
            }
        }

        public void Close()
        {
            _connection.Close();
        }

        private SqliteDataReader SelectQuery(string sql)
        {
            _query.CommandText = sql;
            var result = _query.ExecuteReader();
            return result;
        }

        public void RecordToLog(string typeEvent, string message)
        {
            _connection.Open();
            _query.CommandText = $"INSERT INTO tab_total_log (type_event, date_time_event, user, message)" +
                    $"VALUES ('{typeEvent}', '{DateTime.Now}', '{Environment.UserName}', '{message}')";
            _query.ExecuteNonQuery();
            _connection.Close();            
        }
                
        public string ReadTheLog() 
        {
            _connection.Open();
            var sql = "SELECT * FROM tab_total_log";
            using var result = SelectQuery(sql);

            if (!result.HasRows)
            {
                //Console.WriteLine("Нет данных");
                return "Нет данных";
            }
            else
            {
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
                string log = "";
                foreach (var total in totals)
                    log += total.TypeEvent + " " + total.DateTimeEvent + " " 
                        + total.User + " " + total.Message + "\n";

                return log;

                /*
                foreach (var total in totals)
                       Console.WriteLine($"{total.Id} | {total.TypeEvent} | 
                {total.DateTimeEvent} | {total.User} | {total.Message}");
                _connection.Close();
                */
            }
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
