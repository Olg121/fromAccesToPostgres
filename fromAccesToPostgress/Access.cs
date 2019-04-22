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
           

            using (connection) // ??? 
            {
                connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\db.mdb;"; // Строка подключения 
                string[] restrictions = new string[4]; // параметры ограничения 
                restrictions[3] = "Table"; // Только таблицы пользователя 

                connection.Open(); // открыть подключение 

                // Get list of user tables
                userTables = connection.GetSchema("Tables", restrictions); // названия пользовательских таблиц

                DataTable table = connection.GetSchema("Columns"); // Comming soon

                // Display the contents of the table.  
                DisplayData(table); // Вывод на экран
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
            List<string> tableNames = new List<string>();
            for (int i = 0; i < userTables.Rows.Count; i++)
                tableNames.Add(userTables.Rows[i][2].ToString());
            for (int i = 0; i < userTables.Rows.Count; i++)
                Console.WriteLine(userTables.Rows[i][2].ToString());
            //foreach (DataRow row in data_table.Rows)
            //{
            //    var column_name = row["COLUMN_NAME"].ToString();
            //    var data_type = row["DATA_TYPE"].ToString();
            //    var ordinal_pos = row["ORDINAL_POSITION"].ToString();

            //    //Console.WriteLine("====");
            //    //Console.WriteLine("Столбец: " + column_name);
            //    //Console.WriteLine("Тип данных: " + data_type);
            //    //Console.WriteLine("позиция по порядку: " + ordinal_pos);
            //    //Console.WriteLine("====");


            //}


        }

        private static void DisplayData(DataTable table)
        {
            string text = "TablDogovor"; 
            for(int i = 0; i<table.Rows.Count; i++)
            {
                if (table.Rows[i][table.Columns[2]].ToString() == text)
                {
                    foreach (DataColumn col in table.Columns)
                        Console.WriteLine("{0} = {1}", col.ColumnName, table.Rows[i][col]);
                    Console.WriteLine("============================");
                }
                else
                    continue;
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
        }

        static string Converter(string accessType)
        {

            string postgresType = " ";

            return postgresType;
        }

    }
}
