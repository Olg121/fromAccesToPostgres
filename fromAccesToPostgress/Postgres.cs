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
        static string databaseName = "newbase";
      
        static public void DBCreate()
        {

            NpgsqlConnectionStringBuilder sb = new NpgsqlConnectionStringBuilder();
            sb.Host = "127.0.0.1";
            sb.Port = 5432;
            sb.Username = "postgres";
            sb.Password = "12qwasZX";
            sb.Database = "newbase";
            Console.WriteLine("Server login    :");
          //  loginServer = Console.ReadLine();
            Console.WriteLine("Server password :");
           // passwordServer = Console.ReadLine();
            Console.WriteLine("Database name   :");
           // databaseName = Console.ReadLine(); 
            connectionString = "Server = localhost; Port = 5432; Username = " + loginServer + " ; Password = " + passwordServer;
            Console.WriteLine(sb.ToString()); 

            npgSqlConnection = new NpgsqlConnection(sb.ToString()); 

            try
            {
                npgSqlConnection.Open();
                npgSqlConnection.Close();
            }
            catch
            {
                sb.Database = ""; 
                npgSqlConnection = new NpgsqlConnection(sb.ToString());
                NpgsqlCommand createDb = new NpgsqlCommand("CREATE DATABASE " + databaseName , npgSqlConnection);
                
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
            sb.Database = "newbase";
            //sb.Database = "newbase";
            var con = new NpgsqlConnection(sb.ConnectionString);
            con.Open();
            NpgsqlCommand createTbl = new NpgsqlCommand("create table if not exists " + tableName + " (" + columns + ");", con);
            
           
            var res =createTbl.ExecuteNonQuery();
            con.Close();
        }


        static public void AddRecord(List<List<string>> querys, List<string> rows, string name)
        {

            NpgsqlConnectionStringBuilder sb = new NpgsqlConnectionStringBuilder();
            sb.Host = "127.0.0.1";
            sb.Port = 5432;
            sb.Username = "postgres";
            sb.Password = "12qwasZX";
            sb.Database = "newbase";
            string types;
            string data; 
            npgSqlConnection.ConnectionString = sb.ToString();
            double buf; 
            for (int i = 0; i<querys[0].Count; i++)
            {
                types = ""; 
                data = ""; 
                // string 
                // int 
                // double
                // date

                for (int j = 0; j<querys.Count; j++)
                {
                    if (querys[j][i].ToString() != "")
                    {
                        types += rows[j] + ",";
                        if (double.TryParse(querys[j][i].ToString(), out buf) && !querys[j][i].ToString().Contains(" "))
                        {
                            data += querys[j][i].ToString() + ",";
                        }
                        else
                        {
                            data += "$$" + querys[j][i].ToString() + @"$$" + ',';
                        }
                    }
                }

               data = data.Remove(data.Length - 1);
               types = types.Remove(types.Length - 1);

                npgSqlConnection.Open();

                NpgsqlCommand createRecord = new NpgsqlCommand("insert into " + name + " (" + types + ") values (" + data + " ); ", npgSqlConnection);
                createRecord.ExecuteNonQuery(); 
                npgSqlConnection.Close(); 
               // Console.WriteLine(types);
               // Console.WriteLine(data);

               // Console.WriteLine(); 





            }
         
            return; 
        }

        


    }
}
/*
 *    NpgsqlConnectionStringBuilder sb = new NpgsqlConnectionStringBuilder();
            sb.Host = "127.0.0.1";
            sb.Port = 5432;
            sb.Username = "postgres";
            sb.Password = "12qwasZX";
            sb.Database = "newbase";
            npgSqlConnection.ConnectionString = sb.ToString();

            npgSqlConnection.Open();

          //  NpgsqlCommand createRecord = new NpgsqlCommand("insert into " + tableName + " (" + types + ") values (" + data + " ); ", npgSqlConnection );
            createRecord.ExecuteNonQuery();
            npgSqlConnection.Close(); 
*/