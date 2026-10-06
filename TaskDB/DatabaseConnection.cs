using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskDB
{
    public static class DatabaseConnection
    {
        // Cadena de conexión adaptada para SQL Server Express
        private static readonly string connectionString = @"Server=.\SQLEXPRESS;Database=TaskDB;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}