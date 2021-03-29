using BrookingsIndoorTrainingSystem.Models;
using BrookingsIndoorTrainingSystem.Services.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BrookingsIndoorTrainingSystem.Services.Access
{
    public class DataAcess
    {

        //this is used to connect to the database
        public string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BITS Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        //
        public bool ConcessionsAddItemAmount(ConcessionsModel item)
        {
            string queryString;
            bool success = true;
            //this is the SQL statement to update our item amount. @itemAmount and @itemName are replaced using the function following
            if(item.itemAmount > 0)
            {
                queryString = "UPDATE ConcessionsTable SET Item_Amount += @itemAmount WHERE Item_Name = @itemName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString, connection);

                    //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                    command.Parameters.Add("@itemName", System.Data.SqlDbType.VarChar, 50).Value = item.itemName;
                    command.Parameters.Add("@itemAmount", System.Data.SqlDbType.Int).Value = item.itemAmount;

                    //basically a test to make sure it worked, and catch exception
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                success = true;
            }
            else
            {
                success = false;
        
            }
            return success;
        }

        public bool ConcessionsRemoveItemAmount(ConcessionsModel item)
        {
            string queryString;
            bool success = true;
            //this is the SQL statement to update our item amount. @itemAmount and @itemName are replaced using the function following
            if (item.itemAmount > 0)
            {
                queryString = "UPDATE ConcessionsTable SET Item_Amount -= @itemAmount WHERE Item_Name = @itemName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString, connection);

                    //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                    command.Parameters.Add("@itemName", System.Data.SqlDbType.VarChar, 50).Value = item.itemName;
                    command.Parameters.Add("@itemAmount", System.Data.SqlDbType.Int).Value = item.itemAmount;

                    //basically a test to make sure it worked, and catch exception
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                success = true;
            }
            else
            {
                success = false;

            }
            return success;
        }



        public bool MoveConcessionItem(ConcessionsModel item)
        {
            //temporary only
            string Location = "Storage";
            int Amount = 15;
            //temp ^

            bool passed = true;
            if(Amount > item.itemAmount)
            {
                passed = false;
            }
            else
            {
                if(Amount == item.itemAmount)
                {
                    item.itemLoc = Location;
                }
                else
                {
                    ConcessionsModel newItem = new ConcessionsModel();
                    newItem = item;
                    newItem.itemAmount = Amount;
                    item.itemAmount = item.itemAmount - Amount;
                }
            }
            return passed;
        }
    }
}