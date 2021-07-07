using System;
using Microsoft.Data.Sqlite;
using connectDB;
using DataModel;

namespace Logger
{
    public class LogToDB
    {
        public DBLogger db = new DBLogger();
        public SqliteCommand command = new SqliteCommand();

        public LogToDB() { }

        public void AddRecordsTypesOfEvent(string typeevent)
        {
            db.Open();            
            command.Connection = db._connection;
            command.CommandText = $"INSERT INTO tab_types_of_events (type_event) VALUES ('{typeevent}')";
            db.Close();
        }
    }
}
