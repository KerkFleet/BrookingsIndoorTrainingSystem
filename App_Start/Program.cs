using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BrookingsIndoorTrainingSystem.App_Start
{
    public class Program
    {

        public static void Main()
        {
            bool result;
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BITS Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string sqlCreateDBQuery;
            
            try
            {
               SqlConnection tmpConn = new SqlConnection("server=(local)\\SQLEXPRESS;Trusted_Connection=yes");

                sqlCreateDBQuery = "SELECT database_id FROM sys.databases WHERE Name = 'BITS Database'";
        
                 using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        tmpConn.Close();

                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            if(!result)
            {
                String str;
                SqlConnection myConn = new SqlConnection("Server=localhost;Integrated security=SSPI;database=master");

                str = "CREATE DATABASE BITS Databse;";
                SqlCommand myCommand = new SqlCommand(str, myConn);
                try
                {
                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                    myConn.Close();
  
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }



    }
}