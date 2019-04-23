using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace fromAccesToPostgress
{
    class Postgres
    {
        static NpgsqlConnection npgSqlConnection;
        static string connectionString; 

        static public void DBCreate()
        {
            string loginServer = "login";
            string passwordServer = "password";
            string databaseName = "name"; 
            Console.WriteLine("Server login    :");
            loginServer = Console.ReadLine();
            Console.WriteLine("Server password :");
            passwordServer = Console.ReadLine();
            Console.WriteLine("Database name   :");
            databaseName = Console.ReadLine(); 
            connectionString = "Server = localhost; Port = 5432; Username = " + loginServer + "; Password = " + passwordServer + "; Database = " + databaseName + ";";


            npgSqlConnection = new NpgsqlConnection(connectionString);
            try
            {
                npgSqlConnection.Open();
                npgSqlConnection.Close();
            }
            catch
            {
                npgSqlConnection = new NpgsqlConnection(connectionString);
                NpgsqlCommand createDb = new NpgsqlCommand("create database " + databaseName + ";", npgSqlConnection);
                npgSqlConnection.Open();
                createDb.ExecuteNonQuery();
                npgSqlConnection.Close(); 


            }
            finally
            {
                npgSqlConnection.Close();
            }

        }

        static public void TableCreate(string tableName, string columns)
        {
            NpgsqlCommand createTbl = new NpgsqlCommand("create table " + tableName + " ( " + columns + ");", npgSqlConnection);
            npgSqlConnection.Open();
            createTbl.ExecuteNonQuery();
            npgSqlConnection.Close();
        }



    }
}
