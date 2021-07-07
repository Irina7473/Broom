using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using DataModel;

namespace connectDB
{
    public class DBLogger
    {
        private string connectionString = "Data Source=logger_db.sqlite;Mode=ReadWrite;";

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

        private SqliteDataReader SelectQuery(string sql)
        {
            _query.CommandText = sql;
            var result = _query.ExecuteReader();
            return result;
        }

        /*
        public void AddRecordsTotalLog(int idtypeevent, string message)
        {
            using (var connection = _connection)
            {
                connection.Open();
                string sqlExpression = $"INSERT INTO tab_total_log (id_type_event, date_time_event, user, message)" +
                    $"VALUES ('{idtypeevent}', '{DateTime.Now.ToString()}', '{Environment.UserName.ToString()}', '{message}')";
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                connection.Close();
            }
        }

        public void AddRecordsTypesOfEvent(string typeevent)
        {            
                _connection.Open();   
                string sqlExpression = $"INSERT INTO tab_types_of_events (type_event) VALUES ('{typeevent}')";
                SqliteCommand command = new SqliteCommand(sqlExpression, _connection);
                _connection.Close();            
        }
        */

        public void GetTypesOfEvent()
        {            
            this.Open();
            var sql = "select * from tab_types_of_events";            
            using var result = SelectQuery(sql); 

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            var types = new List<TypesOfEvent>();
            while (result.Read())
            {
                var type = new TypesOfEvent
                {
                    Id = result.GetInt32("id"),
                    TypeEvent = result.GetString("type_event")
                };
                types.Add(type);
            }

            foreach (var type in types)
            {
                Console.WriteLine($"{type.Id} - {type.TypeEvent}");
            }

            this.Close();
        }

        public void GetTotalLog()
        {
            this.Open();
            var sql = "select * from tab_total_log";
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
                    IdTypeEvent = result.GetInt32(1),
                    DateTimeEvent = result.GetString(2),
                    User = result.GetString(3),
                    Message = result.GetString(4)
                };
                totals.Add(total);
            }

            foreach (var total in totals)
            {
                Console.WriteLine($"{total.Id} | {total.IdTypeEvent} | {total.DateTimeEvent} | {total.User} | {total.Message}");
            }

            this.Close();
        }
    }
}
