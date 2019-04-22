using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fromAccesToPostgress
{
    class Program
    {
        static void Main(string[] args)
        {
            //Postgres.DBCreate(); 
            Access.AccessConnect();
        }
    }
}