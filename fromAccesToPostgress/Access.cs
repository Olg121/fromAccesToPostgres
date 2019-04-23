using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

using System.Data;
using System.Data.SqlClient;
namespace fromAccesToPostgress
{
    class Access
    {

        public static void AccessConnect()
        {
            // Microsoft Access provider factory
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            DbConnection connection = factory.CreateConnection();
            DataTable userTables = null;
            DataTable table = null; 

            using (connection) // ??? 
            {
                connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db.mdb;"; // Строка подключения 
                string[] restrictions = new string[4]; // параметры ограничения 
                restrictions[3] = "Table"; // Только таблицы пользователя 

                connection.Open(); // открыть подключение 

                // Get list of user tables
                userTables = connection.GetSchema("Tables", restrictions); // названия пользовательских таблиц

                table = connection.GetSchema("Columns"); // Comming soon
                var table1 = connection.GetSchema("Indexes"); 
                // Display the contents of the table.  
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            List<string> tableNames = new List<string>();
            for (int i = 0; i < userTables.Rows.Count; i++)
                if(userTables.Rows[i][2].ToString()[0] == 'T')
                tableNames.Add(userTables.Rows[i][2].ToString());



            for (int i = 0; i < userTables.Rows.Count; i++)
                if (userTables.Rows[i][2].ToString()[0] == 'T')
                    Console.WriteLine(userTables.Rows[i][2].ToString());
            List<string> answer = Find(table , tableNames); // Вывод на экран
           
            for(int i = 0; i<tableNames.Count; i++)
            {
                Postgres.TableCreate(answer[i].Remove(answer[i].Length - 1), tableNames[i]); 
            }


            

        }

        private static List<string> Find(DataTable table , List<string> tableList)
        {
            string text = null;
            string convetedStringToPostgre = "";
            string curentTableName = null;
            List<string> answer = new List<string>(); 
            string lastTableName = "";
            int k = 0; 
            for(int i = 0; i<table.Rows.Count; i++)
            {

                curentTableName = table.Rows[i][table.Columns[2]].ToString();
                if(curentTableName != lastTableName && convetedStringToPostgre != "" && lastTableName[0] == 'T')
                {
                    k++;
                    Console.WriteLine(k);
                    answer.Add(convetedStringToPostgre);
                    Console.WriteLine(lastTableName); 
                    Console.WriteLine(convetedStringToPostgre);
                    Console.WriteLine();
                    Console.WriteLine(); 
                    convetedStringToPostgre = "";
                }
                if (tableList.Contains(curentTableName)) 
                {
                   convetedStringToPostgre += table.Rows[i][table.Columns[3]].ToString() + ' ' + Converter(table.Rows[i][table.Columns[11]].ToString()) + ','; 
                   //  foreach (DataColumn col in table.Columns)
                   //    Console.WriteLine("{0} = {1}", col.ColumnName, table.Rows[i][col]);
                   // Console.WriteLine(table.Rows[i][table.Columns[11]].ToString()); 
                   
                }
                else
                    continue;
                lastTableName = curentTableName; 
             }

            return answer;
            
        }

        static Dictionary<string, string> typeDictionary = new Dictionary<string, string>() {
                                                                                           { "130", "text" },
                                                                                           { "3", "integer" },
                                                                                           { "11", "boolean" },
                                                                                           { "2", "smallint" },
                                                                                           { "7", "date" },
                                                                                           { "5", "double precision" }
                                                                                         };
        static string Converter(string accessType)
        {

            return typeDictionary[accessType];
        }

    }
}



/*
            foreach (DataRow row in table.Rows)
            {

                foreach (DataColumn col in table.Columns)
                {
                    Console.WriteLine("{0} = {1}", col.ColumnName, row[col]);
                }
                Console.WriteLine("============================");
            }
            */
