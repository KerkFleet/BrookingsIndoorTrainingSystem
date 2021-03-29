using BrookingsIndoorTrainingSystem.Models;
using BrookingsIndoorTrainingSystem.Services.Access;
using BrookingsIndoorTrainingSystem.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrookingsIndoorTrainingSystem.Controllers
{
    public class HomeController : Controller
    {
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
        // Go to Storage item page
        public ActionResult StorageView()
        {
            return View("StorageView");
        }


        // Go to Concessions Add item page
        public ActionResult ConcessionsAddItemView()
        {
            return View("ConcessionsAddItemView");
        }

        // Go to Concessions remove item page
        public ActionResult ConcessionsRemoveItemView()
        {
            return View("ConcessionsRemoveItemView");
        }


        public string ConcessionsAddSoda(ConcessionsModel concessionsModel)
        {
            concessionsModel.itemName = "Sodas";
            DataAcess dataAccess = new DataAcess();
            Boolean success = dataAccess.ConcessionsAddItemAmount(concessionsModel);
            if (success)
                return "Item added";
            else
                return "Please enter an integer value greater than zero.";
        }
        public string ConcessionsRemoveSoda(ConcessionsModel concessionsModel)
        {
            concessionsModel.itemName = "Sodas";
            DataAcess dataAccess = new DataAcess();
            Boolean success = dataAccess.ConcessionsRemoveItemAmount(concessionsModel);
            if (success)
                return "Item removed";
            else
                return "Please enter an integer value greater than zero, but less or equal to the total amount.";
        }


        public string ConcessionsUpdateItemLoc(ConcessionsModel concessionsModel)
        {


            DataAcess dataAccess = new DataAcess();
            Boolean success = dataAccess.MoveConcessionItem(concessionsModel);
            if (success)
                return "Correct Amount";
            else
                return "Incorrect Amount";
        }
    }
}