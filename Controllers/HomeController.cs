using BrookingsIndoorTrainingSystem.Models;
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
            // Initalize Cart
            GlobalConcessionsCartModel.cart = new List<ConcessionsModel>();
            GlobalConcessionsCartModel.total = 0;
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

        // equipment Update
        public ActionResult EquipmentMoveItemView()
        {
            return View("EquipmentMoveItemView");
        }

        // equipment Rent item view
        public ActionResult EquipmentRentItemView()
        {
            return View("EquipmentRentItemView");
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
                    double itemPrice=0;
                    bool exists;
                    exists = false;
                    int existingID = 0;
                    reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        location = (string)reader["Item_Loc"];
                        itemPrice = (double)reader["Item_Price"];
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
                        else
                        {
                            success = false;
                            ViewBag.Error = "Error: Amount to be moved must be positive and less than or equal to the current amount.\nCurrent amount: " + current_amount.ToString();
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
                            command.CommandText = "INSERT INTO ConcessionsTable values(@iN, @iA, @iL, @iP)";
                            command.Parameters.Add("@iN", System.Data.SqlDbType.VarChar, 50).Value = itemName;
                            command.Parameters.Add("@iA", System.Data.SqlDbType.Int).Value = item.itemAmount;
                            command.Parameters.Add("@iL", System.Data.SqlDbType.VarChar, 50).Value = item.itemLoc;
                            command.Parameters.Add("@iP", System.Data.SqlDbType.Float).Value = itemPrice;

                            command.ExecuteNonQuery();
                            command.CommandText = "UPDATE ConcessionsTable SET Item_Amount -= @itemAmount WHERE Id = @id2";
                            command.Parameters.Add("@itemAmount", System.Data.SqlDbType.Int).Value = item.itemAmount;
                            command.Parameters.Add("@id2", System.Data.SqlDbType.Int).Value = id;
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            success = false;
                            ViewBag.Error = "Error: Amount to be moved must be positive and less than or equal to the current amount.";
                            ViewBag.Error2 = "Current amount: " + current_amount.ToString();
                        }
                    }

    
                }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            if (success)
                return View("ConcessionsView");
            else
            {
                ViewBag.itemName = itemName;
                ViewBag.id = id;
                return View("ConcessionsUpdateLocation");
            }

        }

        public ActionResult ConcessionsAddItemView(string itemName, int id)
        {
            ViewBag.itemName = itemName;
            ViewBag.id = id;
            return View();
        }

        // Go to Concessions remove item page
        public ActionResult ConcessionsRemoveItemView(string itemName, int id)
        {
            ViewBag.itemName = itemName;
            ViewBag.id = id;
            //StorageView();
            return View("ConcessionsRemoveItemView");
        }


        public ActionResult ConcessionsAddItem(ConcessionsModel item, string itemName, int id)
        {
            string queryString;
            bool success = true;
            
            //this is the SQL statement to update our item amount. @itemAmount and @itemName are replaced using the function following
            if (item.itemAmount > 0 && item.itemAmount <= 500)
            {
                queryString = "UPDATE ConcessionsTable SET Item_Amount += @itemAmount WHERE Id = @id2";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString, connection);

                    //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                    command.Parameters.Add("@id2", System.Data.SqlDbType.Int).Value = id;
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

            }
            else
            {
                success = false;

            }

            if (success)
            {             
                return View("ConcessionsView");
            }
            else
            {
                ViewBag.Error = "Error: Item amount must be a positive integer less than 500.";
                ViewBag.itemName = itemName;
                ViewBag.id = id;
                return View("ConcessionsAddItemView");
            }

        }

        public ActionResult ConcessionsRemoveItem(ConcessionsModel item, string itemName, int id)
        {
            string queryString2;
            string queryString1;
            bool success = true;
            //this is the SQL statement to update our item amount. @itemAmount and @itemName are replaced using the function following
            if (item.itemAmount > 0)
            {
                queryString1 = "Select Item_Amount FROM ConcessionsTable WHERE Id = @id1";
                queryString2 = "UPDATE ConcessionsTable SET Item_Amount -= @itemAmount WHERE Id = @id2";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString1, connection);
                    command.Parameters.Add("@id1", System.Data.SqlDbType.Int).Value = id;



                    //basically a test to make sure it worked, and catch exception
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        int current_val = (int)reader["Item_Amount"];
                        if (current_val < item.itemAmount)
                        {
                            success = false;
                            ViewBag.Error = "Error: Cannot remove more than current value. Current value is: " + current_val;
                        }
                    
                        if(success)
                        {
                            connection.Close();
                            connection.Open();
                            command.CommandText = queryString2;
                            //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                            command.Parameters.Add("@id2", System.Data.SqlDbType.Int).Value = id;
                            command.Parameters.Add("@itemAmount", System.Data.SqlDbType.Int).Value = item.itemAmount;
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            else
            {
                ViewBag.Error = "Error: Item amount must be a positive integer.";
                success = false;
            }


            if (success)
            {
                return View("ConcessionsView");
            }
            else
            {

                ViewBag.itemName = itemName;
                ViewBag.id = id;
               return View("ConcessionsRemoveItemView");
            }
        }


        //public ActionResult ConcessionsUpdateItemLoc(ConcessionsModel concessionsModel)
        //{


        //    DataAcess dataAccess = new DataAcess();
        //    concessionsModel.itemName = "Sodas";
        //    Boolean success = dataAccess.MoveConcessionItem(concessionsModel);
        //    if (success)
        //        return View("StorageView");

        //    return View("ConcessionsView");
        //}

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
                    item.itemPrice = (double)rdr["Item_Price"];
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

        public ActionResult ConcessionsMakeSaleView()
        {
            string sql = "SELECT * FROM ConcessionsTable";



            var model = new List<ConcessionsModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);




                connection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var item = new ConcessionsModel();
                    item.id = (int)rdr["Id"];
                    item.itemName = (string)rdr["Item_Name"];
                    item.itemAmount = (int)rdr["Item_Amount"];
                    item.itemLoc = (string)rdr["Item_Loc"];
                    item.itemPrice = (double)rdr["Item_Price"];
                    model.Add(item);

                }
            }
            return View(model);
        }

        public ActionResult ConcessionsCartView()
        {

            var model = GlobalConcessionsCartModel.cart;

            return View(model);
        }

        public ActionResult ConcessionsCartAddItemView(string itemName, int id, double itemPrice)
        {
            ViewBag.itemName = itemName;
            ViewBag.id = id;
            ViewBag.itemPrice = itemPrice;
            return View();
        }

        public ActionResult ConcessionsCartAddItem(ConcessionsModel item, string itemName, int id, double itemPrice)
        {
            string queryString1;
            string queryString;
            bool success = true;

            //this is the SQL statement to update our item amount. @itemAmount and @itemName are replaced using the function following
            if (item.itemAmount > 0)
            {
                queryString1 = "Select Item_Amount FROM ConcessionsTable WHERE Id = @id1";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString1, connection);
                    command.Parameters.Add("@id1", System.Data.SqlDbType.Int).Value = id;

                    //basically a test to make sure it worked, and catch exception
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        int current_val = (int)reader["Item_Amount"];
                        if (current_val < item.itemAmount)
                        {
                            ViewBag.Error = "Error: There is not enough of that item";

                            ConcessionsCartAddItemView(itemName, id, item.itemPrice);
                            return View("ConcessionsCartAddItemView");
                        }
                        connection.Close();

                        if (success)
                        {

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                        

                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    queryString = "UPDATE ConcessionsTable SET Item_Amount -= @itemAmount WHERE Id = @id";

                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString, connection);

                    //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    command.Parameters.Add("@itemAmount", System.Data.SqlDbType.Int).Value = item.itemAmount;

                    //basically a test to make sure it worked, and catch exception
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                ViewBag.Error = "";
                GlobalConcessionsCartModel.cart.Add(item);
                GlobalConcessionsCartModel.total += itemPrice*item.itemAmount;
                ConcessionsCartView();
                return View("ConcessionsCartView");
            }
            else
            {
                ViewBag.Error = "Error: Can not add negative item amounts";

                ConcessionsCartAddItemView(itemName, id, item.itemPrice);
                return View("ConcessionsCartAddItemView");
            }

        }

        public ActionResult ConcessionsCartRemoveItem(string itemName, int id)
        {
            for (int i = 0; i < GlobalConcessionsCartModel.cart.Count; i++)
            {
                // if it is List<String>
                if (GlobalConcessionsCartModel.cart[i].id == id)
                {

                    string queryString;
                    bool success = true;

                    //this is the SQL statement to update our item amount. @itemAmount and @itemName are replaced using the function following

                        queryString = "UPDATE ConcessionsTable SET Item_Amount += @itemAmount WHERE Id = @id";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            //here is where we connect to the database and perform the SQL command
                            SqlCommand command = new SqlCommand(queryString, connection);

                            //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                            command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                            command.Parameters.Add("@itemAmount", System.Data.SqlDbType.Int).Value = GlobalConcessionsCartModel.cart[i].itemAmount;

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

                    GlobalConcessionsCartModel.total -= GlobalConcessionsCartModel.cart[i].itemPrice * GlobalConcessionsCartModel.cart[i].itemAmount;
                    GlobalConcessionsCartModel.cart.RemoveAt(i);

                }
            }
            ConcessionsCartView();
            return View("ConcessionsCartView");
        }
        
        public ActionResult ConcessionsClearCart()
        {
            // Update Funds Database
            FundsAddtoFunds(GlobalConcessionsCartModel.total, "Concessions");

            for (int i = 0; i < GlobalConcessionsCartModel.cart.Count;)
            {
                GlobalConcessionsCartModel.cart.RemoveAt(i);
            }
            GlobalConcessionsCartModel.total = 0;
            ConcessionsMakeSaleView();

            return View("ConcessionsMakeSaleView");
        }

        // ++++++++++++++++++++++++++++ FUNDS Controllers  ++++++++++++++++++++++++++ //
        public ActionResult FundsMakePaymentView(string itemName)
        {
            ViewBag.itemName = itemName;
            return View();
        }

        public ActionResult FundsView()
        {
            string sql = "SELECT * FROM Funds";

            var model = new FundsModel();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var item = new FundsModel();
                    item.id = (int)rdr["Id"];
                    item.Concessions = (double)rdr["Concessions"];
                    item.BITS = (double)rdr["BITS"];
                    item.Equipment = (double)rdr["Equipment"];
                    item.Space = (double)rdr["Space"];
                    model = item;
                }
            }
            return View(model);
        }

        public ActionResult FundsUpdateConcesssionsView(double funds)
        {

            var model = new FundsModel();
            model.Concessions = funds;

            return View(model);
        }

        public ActionResult FundsUpdateEquipmentView(double funds)
        {

            var model = new FundsModel();
            model.Equipment = funds;

            return View(model);
        }

        public ActionResult FundsUpdateSpaceView(double funds)
        {

            var model = new FundsModel();
            model.Space = funds;

            return View(model);
        }

        public ActionResult FundsUpdate(FundsModel funds, string location)
        {
            string queryString3 = "";
            string queryString2 = "";
            string queryString1 = "";
            bool success = true;
            double amount = 0;
            if (location == "Concessions")
            {
                amount = funds.Concessions;
                queryString2 = "UPDATE Funds SET Concessions = @Amount WHERE Id = 1";
            }
            
            else if (location == "Equipment")
            {
                amount = funds.Equipment;
                queryString2 = "UPDATE Funds SET Equipment = @Amount WHERE Id = 1";
            }
          
            else if (location == "Space")
            {
                amount = funds.Space;
                queryString2 = "UPDATE Funds SET Space = @Amount WHERE Id = 1";
            }
               
            //this is the SQL statement to update our item amount. @itemAmount and @itemName are replaced using the function following
            if (amount > 0)
            {




                queryString1 = "SELECT * FROM Funds";
                queryString3 = "UPDATE Funds SET BITS += @difference WHERE Id = 1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //here is where we connect to the database and perform the SQL command
                    SqlCommand command = new SqlCommand(queryString1, connection);

                    //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables


                    //basically a test to make sure it worked, and catch exception
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        reader.Read();
                        double current = (double)reader[location];
                        double difference;
                        difference = current - amount;
                        connection.Close();
                        connection.Open();
                        command.CommandText = queryString3;
                        command.Parameters.Add("@difference", System.Data.SqlDbType.Float).Value = difference;
                        command.ExecuteNonQuery();
                        connection.Close();
                        connection.Open();
                        command.CommandText = queryString2;
                        command.Parameters.Add("@Amount", System.Data.SqlDbType.Float).Value = amount;
                        command.ExecuteNonQuery();

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

            FundsView();
            return View("FundsView");
        }

        public void FundsAddtoFunds(double amount, string location)
        {
            // Get old amount
            double oldAmount = 0;
            string sql = "SELECT * FROM Funds";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    oldAmount = (double)rdr["Concessions"];
                }
            }
            // Get old amount

            // Get new amount
            double newAmount = oldAmount + amount;

            // Update new amount
            FundsModel funds = new FundsModel();
            funds.Concessions = newAmount;
            FundsUpdate(funds, location);
        }

        public ActionResult ScheduleUpdateEmployeeView(int Id, string Dates, string Monday, string Tuesday, string Wednesday, string Thursday, string Friday, string Saturday)
        {
            ScheduleModel employee = new ScheduleModel();
            ViewBag.Id = Id;
            ViewBag.Dates = Dates;
            employee.Id = Id;
            employee.Dates = Dates;
            employee.Monday = Monday;
            employee.Tuesday = Tuesday;
            employee.Wednesday = Wednesday;
            employee.Thursday = Thursday;
            employee.Friday = Friday;
            employee.Saturday = Saturday;
            return View(employee);
        }

        // ++++++++++++++++++++++++++++ FUNDS Controllers  ++++++++++++++++++++++++++ //
        public ActionResult ScheduleEmployeeView()
        {
            string sql = "SELECT * FROM Schedule";



            var model = new List<ScheduleModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);




                connection.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var item = new ScheduleModel();
                    item.Id = (int)rdr["Id"];
                    item.Dates = (string)rdr["Dates"];
                    item.Monday = (string)rdr["Monday"];
                    item.Tuesday = (string)rdr["Tuesday"];
                    item.Wednesday = (string)rdr["Wednesday"];
                    item.Thursday = (string)rdr["Thursday"];
                    item.Friday = (string)rdr["Friday"];
                    item.Saturday = (string)rdr["Saturday"];
    

                    model.Add(item);

                }
            }
            return View(model);
        }

        public ActionResult UpdateSchedule(ScheduleModel employee, int Id, string Dates)
        {
            string queryString = "UPDATE Schedule SET Monday = @mon, Tuesday = @tues, Wednesday = @wed, Thursday = @thurs, Friday = @fri, Saturday = @sat WHERE Id = @id";
            employee.Id = Id;
            employee.Dates = Dates;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //here is where we connect to the database and perform the SQL command
                SqlCommand command = new SqlCommand(queryString, connection);

                //thesee statements replace the @itemName and @itemAmount in the queryString with their appropriate variables
                command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = employee.Id;
                command.Parameters.Add("@mon", System.Data.SqlDbType.VarChar, 50).Value = employee.Monday;
                command.Parameters.Add("@tues", System.Data.SqlDbType.VarChar, 50).Value = employee.Tuesday;
                command.Parameters.Add("@wed", System.Data.SqlDbType.VarChar, 50).Value = employee.Wednesday;
                command.Parameters.Add("@thurs", System.Data.SqlDbType.VarChar, 50).Value = employee.Thursday;
                command.Parameters.Add("@fri", System.Data.SqlDbType.VarChar, 50).Value = employee.Friday;
                command.Parameters.Add("@sat", System.Data.SqlDbType.VarChar, 50).Value = employee.Saturday;


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

            ScheduleEmployeeView();
            return View("ScheduleEmployeeView");
        }


    }
}