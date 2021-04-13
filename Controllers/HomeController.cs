﻿using BrookingsIndoorTrainingSystem.Models;
using BrookingsIndoorTrainingSystem.Services.Access;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BrookingsIndoorTrainingSystem.Controllers
{

    public class HomeController : Controller
    {
        //Shelby's string
        public string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BITS Database;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //+++++++++++++++++ Home pages Controllers +++++++++++++++++++++++++++++++++
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is the about page";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "This is the contact page";

            return View();
        }

        //+++++++++++ Equipment Controllers +++++++++++++++++++++++++++++++
        public ActionResult EquipmentView()
        {
            return View("EquipmentView");
        }
        // equipment add item view
        public ActionResult EquipmentAddItemView()
        {
            return View("EquipmentAddItemView");
        }

        // equipment Remove item view
        public ActionResult EquipmentRemoveItemView()
        {
            return View("EquipmentRemoveItemView");
        }

        // ++++++ SPACES Controllers  +++++++++++++++++++++++++++++++++++++++++++++++
        public ActionResult SpaceView()
        {
            return View("SpaceView");
        }

        // Added space soccer view - Nate O'Meara
        public ActionResult SpaceSoccerView()
        {
            return View("SpaceSoccerView");
        }

        // Go to Track page
        public ActionResult SpaceTrackView()
        {
            return View("SpaceTrackView");
        }

        // Go to Batting Cage page
        public ActionResult SpaceBattingView()
        {
            return View("SpaceBattingView");
        }


        // +++++ Concessions Controllers +++++++++++++++++++++++++++++++++++++++++++
        public ActionResult ConcessionsView()
        {
            return View("ConcessionsView");
        }

        public ActionResult ConcessionsUpdateLocationView(ConcessionsModel concessionsModel)
        {

            return View("ConcessionsUpdateLocation");
        }

        // ++ Added John Kirkvold


        // Go to Concessions Add item page
        public ActionResult ConcessionsUpdateLocation(string itemName, int id)
        {
            ViewBag.itemName = itemName;
            ViewBag.id = id;
            return View();
        }

        public ActionResult ConcessionsMoveItem(ConcessionsModel item, string itemName, int id)
        {
            int current_amount;
            string queryString;
            bool success = true;

                queryString = "SELECT * FROM ConcessionsTable WHERE Id = @id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString, connection);

                    //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    
                    //basically a test to make sure it worked, and catch exception
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();      
                        reader.Read();
                        current_amount = (int)reader["Item_Amount"];
                        connection.Close();
                        connection.Open();
                        command.CommandText = "SELECT * FROM ConcessionsTable WHERE Item_Name = @itemName1";
                        command.Parameters.Add("@itemName1", System.Data.SqlDbType.VarChar, 50).Value = itemName;

                   
                    string location;
                    bool exists;
                    exists = false;
                    int existingID = 0;
                    reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        location = (string)reader["Item_Loc"];
                        if (location == item.itemLoc)
                        {
                            exists = true;
                            existingID = (int)reader["Id"];
                            break;
                        }
                           
                    }
                    connection.Close();
                    connection.Open();
                    if(exists)
                    {
                        if (item.itemAmount == current_amount)
                        {

                            // Change the SQL Command and execute
                            command.CommandText = "DELETE FROM ConcessionsTable WHERE Id = @id2;";                         
                            command.Parameters.Add("@id2", System.Data.SqlDbType.Int).Value = id;
                            command.ExecuteNonQuery();
                            command.CommandText = "UPDATE ConcessionsTable SET Item_Amount += @itemAmount WHERE Id = @existingID";
                            command.Parameters.Add("@itemAmount", System.Data.SqlDbType.Int).Value = item.itemAmount;
                            command.Parameters.Add("@existingID", System.Data.SqlDbType.Int).Value = existingID;

                            command.ExecuteNonQuery();
                        }
                        else if (item.itemAmount < current_amount && item.itemAmount > 0)
                        {
                            command.CommandText = "UPDATE ConcessionsTable SET Item_Amount += @itemAmount WHERE Id = @existingID";
                            command.Parameters.Add("@itemAmount", System.Data.SqlDbType.Int).Value = item.itemAmount;
                            command.Parameters.Add("@existingID", System.Data.SqlDbType.Int).Value = existingID;
                            command.ExecuteNonQuery();
                            command.CommandText = "UPDATE ConcessionsTable SET Item_Amount -= @itemAmount2 WHERE Id = @id2";
                            command.Parameters.Add("@itemAmount2", System.Data.SqlDbType.Int).Value = item.itemAmount;
                            command.Parameters.Add("@id2", System.Data.SqlDbType.Int).Value = id;
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (item.itemAmount == current_amount)
                        {

                            // Change the SQL Command and execute
                            command.CommandText = "UPDATE ConcessionsTable SET Item_Loc = @itemLoc WHERE Id = @id2";
                            command.Parameters.Add("@itemLoc", System.Data.SqlDbType.VarChar, 50).Value = item.itemLoc;
                            command.Parameters.Add("@id2", System.Data.SqlDbType.Int).Value = id;
                            command.ExecuteNonQuery();
                        }
                        else if (item.itemAmount < current_amount && item.itemAmount > 0)
                        {
                            command.CommandText = "INSERT INTO ConcessionsTable values(@iN, @iA, @iL)";
                            command.Parameters.Add("@iN", System.Data.SqlDbType.VarChar, 50).Value = itemName;
                            command.Parameters.Add("@iA", System.Data.SqlDbType.Int).Value = item.itemAmount;
                            command.Parameters.Add("@iL", System.Data.SqlDbType.VarChar, 50).Value = item.itemLoc;
                            command.ExecuteNonQuery();
                            command.CommandText = "UPDATE ConcessionsTable SET Item_Amount -= @itemAmount WHERE Id = @id2";
                            command.Parameters.Add("@itemAmount", System.Data.SqlDbType.Int).Value = item.itemAmount;
                            command.Parameters.Add("@id2", System.Data.SqlDbType.Int).Value = id;
                            command.ExecuteNonQuery();
                        }
                    }

    
                }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                

            return View("ConcessionsView");
        }

        public ActionResult ConcessionsAddItemView(string itemName)
        {
            ViewBag.itemName = itemName;
            return View();
        }

        // Go to Concessions remove item page
        public ActionResult ConcessionsRemoveItemView(string itemName)
        {
            ViewBag.itemName = itemName;
            return View("ConcessionsRemoveItemView");
        }


        public ActionResult ConcessionsAddItem(ConcessionsModel item, string itemName)
        {
            string queryString;
            bool success = true;
            
            //this is the SQL statement to update our item amount. @itemAmount and @itemName are replaced using the function following
            if (item.itemAmount > 0)
            {
                queryString = "UPDATE ConcessionsTable SET Item_Amount += @itemAmount WHERE Item_Name = @itemName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString, connection);

                    //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                    command.Parameters.Add("@itemName", System.Data.SqlDbType.VarChar, 50).Value = itemName;
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

            return View("ConcessionsView");
            //if (success)

            //    StorageView();
            //else
            //{
            //    ConcessionsAddItemView("itemName");
            //}

        }

        public ActionResult ConcessionsRemoveItem(ConcessionsModel item, string itemName)
        {
            string queryString2;
            bool success = true;
            //this is the SQL statement to update our item amount. @itemAmount and @itemName are replaced using the function following
            if (item.itemAmount > 0)
            {
             
                queryString2 = "UPDATE ConcessionsTable SET Item_Amount -= @itemAmount WHERE Item_Name = @itemName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString2, connection);

                    //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                    command.Parameters.Add("@itemName", System.Data.SqlDbType.VarChar, 50).Value = itemName;
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

            return View("ConcessionsView");
            //if (success)

            //    StorageView();
            //else
            //{
            //    ConcessionsAddItemView("itemName");
            //}
        }


        public ActionResult ConcessionsUpdateItemLoc(ConcessionsModel concessionsModel)
        {


            DataAcess dataAccess = new DataAcess();
            concessionsModel.itemName = "Sodas";
            Boolean success = dataAccess.MoveConcessionItem(concessionsModel);
            if (success)
                return View("StorageView");

            return View("ConcessionsView");
        }

        // GET: /concessionsTable/ 

        public ActionResult StorageView()
        {

            //this is used to connect to the database
            //Shelby's string

            string sql = "SELECT * FROM ConcessionsTable";

            var model = new List<ConcessionsModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);


      

                connection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read())
                {
                    var item = new ConcessionsModel();
                    item.id = (int)rdr["Id"];
                    item.itemName = (string)rdr["Item_Name"];
                    item.itemAmount = (int)rdr["Item_Amount"];
                    item.itemLoc = (string)rdr["Item_Loc"];
                    model.Add(item);

                }
            }
            return View(model);
        }

        public ActionResult ConcessionsEditItemView(string itemName, int id)
        {
            ViewBag.itemName = itemName;
            ViewBag.id = id;
            return View();
        }
    }
}