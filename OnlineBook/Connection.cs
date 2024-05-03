using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace OnlineBook
{
   
    //singleton design pattern

    public sealed class DatabaseConnection
    {
        private static readonly Lazy<DatabaseConnection> instance =
            new Lazy<DatabaseConnection>(() => new DatabaseConnection());

        private readonly SqlConnection connection;

        
        private DatabaseConnection()
        {
            string connectionString = "LAPTOP-TPKNAAAB";
            connection = new SqlConnection(connectionString);
        }

        // Public method to provide access to the singleton instance
        public static DatabaseConnection Instance => instance.Value;

        // Public method to open the database connection
        public void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                Console.WriteLine("Database connection opened.");
            }
        }

        // Public method to close the database connection
        public void CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
                Console.WriteLine("Database connection closed.");
            }
        }

    }
}
