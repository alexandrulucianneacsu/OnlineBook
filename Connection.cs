using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Testing

{
    //this cod is after using singleton design pattern
    public class Connection
    {
        private static Connection instance;
        private readonly SqlConnection connection;

        private Connection()
        {
            // Establish the SQL connection
            connection = new SqlConnection("Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;");
        }

        public static Connection GetInstance()
        {
            // Create a new instance only if it's not already created
            if (instance == null)
            {
                instance = new Connection();
            }
            return instance;
        }

        public SqlConnection ConnectionOpen()
        {
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                // Handle exceptions
            }
            return connection;
        }

        public void ConnectionClose()
        {
            try
            {
                connection.Close();
            }
            catch (Exception)
            {
                // Handle exceptions
            }
        }
    }
}

//This cod is before using singleton

/* class Connection
{
  readonly SqlConnection connection;

 // Establish the SQL connection

  public Connection() => connection = new SqlConnection
          ("Data Source=LAPTOP-TPKNAAAB; Initial Catalog=OnlineBook; Integrated Security=True;");
  public SqlConnection ConnectionOpen()
  {
      try
      {
          connection.Open();
      }
      catch (Exception)
      {


      }
      return connection;
  }
  public void ConnectionClose()
  { 
      try
      {
          connection.Close();
      }
      catch (Exception)
      {

      }
  }
}*/


