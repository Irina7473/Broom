using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using connectDB;
using DataModel;

namespace Logger
{
    public class LogToDB
    {
        public DBLogger db = new();
        public SqliteCommand command = new();

        public LogToDB() { }              

        public void RecordEventToDB(string typeevent, string message)
        {
            db.Open();
            command.Connection = db._connection;
            command.CommandText = $"INSERT INTO tab_total_log (type_event, date_time_event, user, message)" +
                $"VALUES ('{typeevent}', '{DateTime.Now.ToString()}', '{Environment.UserName.ToString()}', '{message}')";
            command.ExecuteNonQuery();
            db.Close();
        }

        //Выводит данные таблицы в консоль на этапе отладки
        public void GetTotalLog()
        {
            db.Open();
            var sql = "select * from tab_total_log";
            using var result = db.SelectQuery(sql);

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
            {
                Console.WriteLine($"{total.Id} | {total.TypeEvent} | {total.DateTimeEvent} | {total.User} | {total.Message}");
            }

            db.Close();
        }
    }
}