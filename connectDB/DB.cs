using System;
using System.Data.SqlClient;

namespace connectDB
{
    public class DB
    {
        //не знаю как подключить локальную базу
            string connectionString = "Data Source=logger_db.sqlite;Mode=ReadWrite;";

            private SqlConnection _connection;
            private SqlCommand _query;

            public DB()
            {
                _connection = new SqlConnection(connectionString);
                _query = new SqlCommand
                {
                    Connection = _connection
                };
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
                catch (SqlException)
                {
                    throw new Exception("Подключаемся к уже открытой БД");
                }
            }

            //TODO Дополнить метод проверками исключений
            public void Close()
            {
                _connection.Close();
            }

            private SqlDataReader SelectQuery(string sql)
            {
                _query.CommandText = sql;
                var result = _query.ExecuteReader();
                return result;
            }                      
        }
    }