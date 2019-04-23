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
        
        static string loginServer = "postgres";
        static string passwordServer = "12qwasZX";
        static string databaseName = "newBase"; 
        static public void DBCreate()
        {
            Console.WriteLine("Server login    :");
          //  loginServer = Console.ReadLine();
            Console.WriteLine("Server password :");
           // passwordServer = Console.ReadLine();
            Console.WriteLine("Database name   :");
           // databaseName = Console.ReadLine(); 
            connectionString = "Server = localhost; Port = 5432; Username = " + loginServer + "; Password = " + passwordServer;


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

        static public void TableCreate(string columns,string tableName)
        {
            // connectionString = "Server = localhost; Port = 5432; Username = " + loginServer + "; Password = " + passwordServer + "; Database=" + databaseName;
            //  
            NpgsqlConnectionStringBuilder sb = new NpgsqlConnectionStringBuilder();
            sb.Host = "127.0.0.1";
            sb.Port = 5432;
            sb.Username = "postgres";
            sb.Password = "12qwasZX";
            sb.Database = "newBase";
            //sb.Database = "newbase";
            var con = new NpgsqlConnection(sb.ConnectionString);
            con.Open();
            NpgsqlCommand createTbl = new NpgsqlCommand("create table if not exists " + tableName + " (" + columns + ");", con);
            
           
            var res =createTbl.ExecuteNonQuery();
            con.Close();
        }



    }
}
