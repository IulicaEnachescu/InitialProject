using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace DataAccessConnection
{
    public class ConnectionManager
    {
        private static SqlConnection connection = null;
        private ConnectionManager()
        { }
        public static SqlConnection GetConnection()

        {
            if (connection == null)
            {
                string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

                //   string connectionString2 = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;

                connection = new SqlConnection(connectionString);
                connection.Open();
            }

            return connection;
        }
        public static SqlConnection OpenConnection(SqlConnection connection)
        {
            if (connection == null) return null;
            else
            {
                connection.Open();
                return connection;
            }

        }

    }
}

