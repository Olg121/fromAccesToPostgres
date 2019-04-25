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
        class My_Stuct
        {
            public List<string> First;
            public List<List<string>> Second; 
        }
        static string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=db.mdb;";
        public static void AccessConnect()
        {
           List<List<string>> tableRows = new List<List<string>>();
            // Microsoft Access provider factory
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            DbConnection connection = factory.CreateConnection();
            DataTable userTables = null;
            DataTable table = null;
            List<string> oneRow = new List<string>();
            using (connection) // ??? 
            {
                connection.ConnectionString = connectionString; // Строка подключения 
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


            My_Stuct returnAnswer = new My_Stuct(); 
            for (int i = 0; i < userTables.Rows.Count; i++)
                if (userTables.Rows[i][2].ToString()[0] == 'T')
                    Console.WriteLine(userTables.Rows[i][2].ToString());
            returnAnswer = Find(table , tableNames); // Вывод на экран
            List<string> answer = returnAnswer.First;
            tableRows = returnAnswer.Second; 
            for (int i = 0; i<tableNames.Count; i++)
            {
                Postgres.TableCreate(answer[i].Remove(answer[i].Length - 1), tableNames[i]); 
            }

            OleDbConnection myConnection;
            string recordString = "";
            string recordStringOfTypes = "";
            string recordStringOfData = "";
           
            double buf; 
            for (int i = 0; i < tableNames.Count; i++)
            {
                myConnection = new OleDbConnection(connectionString);
                myConnection.Open();
                string query = "SELECT * FROM " + tableNames[i];

                // создаем объект OleDbCommand для выполнения запроса к БД MS Access
                OleDbCommand command = new OleDbCommand(query, myConnection);

                OleDbDataReader reader = command.ExecuteReader(); 
               
                while(reader.Read())
                {
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        if (reader[j].ToString() != "")
                        {

                            recordStringOfTypes += tableRows[i][j] + ","; 
                            if (double.TryParse(reader[j].ToString(), out buf))
                            {
                                recordStringOfData += reader[j].ToString() + ","; 
                            }
                             else
                            {
                                recordStringOfData += @"'" + reader[j].ToString() + @"' " + ','; 
                            }
                            Console.WriteLine(reader[j]);
                            Console.WriteLine(tableRows[i][j]);
                        }
                     }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();

                   // Postgres.AddRecord(tableNames[i], recordStringOfData.Remove(recordStringOfData.Length - 1), recordStringOfTypes.Remove(recordStringOfTypes.Length - 1)); 

                }
            }

        }


        private static My_Stuct Find(DataTable table , List<string> tableList)
        {
            List<List<string>> tableRows; 
            List<string> oneRow = new List<string>(); 
            string text = null;
            tableRows = new List<List<string>>(); 
            string convetedStringToPostgre = "";
            string curentTableName = null;
            List<string> answer = new List<string>(); 
            string lastTableName = "";
            My_Stuct ans = new My_Stuct(); 
            int k = 0; 
            for(int i = 0; i<table.Rows.Count; i++)
            {

                curentTableName = table.Rows[i][table.Columns[2]].ToString();
                if(curentTableName != lastTableName && convetedStringToPostgre != "" && lastTableName[0] == 'T')
                {
                    k++;
                    Console.WriteLine(k);
                    answer.Add(convetedStringToPostgre);
                    tableRows.Add(new List<string>(oneRow)); 
                    oneRow.Clear(); 
                    Console.WriteLine(lastTableName); 
                    Console.WriteLine(convetedStringToPostgre);
                    Console.WriteLine();
                    Console.WriteLine(); 
                    convetedStringToPostgre = "";
                }
                if (tableList.Contains(curentTableName)) 
                {
                    oneRow.Add(table.Rows[i][table.Columns[3]].ToString());
                   convetedStringToPostgre += table.Rows[i][table.Columns[3]].ToString() + ' ' + Converter(table.Rows[i][table.Columns[11]].ToString()) + ','; 
                   //  foreach (DataColumn col in table.Columns)
                   //    Console.WriteLine("{0} = {1}", col.ColumnName, table.Rows[i][col]);
                   // Console.WriteLine(table.Rows[i][table.Columns[11]].ToString()); 
                   
                }
                else
                    continue;
                lastTableName = curentTableName; 
             }
            ans.First = answer;
            ans.Second = tableRows;
            return ans;
            
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
