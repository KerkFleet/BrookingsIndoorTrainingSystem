using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrookingsIndoorTrainingSystem.Controllers
{
    public class HomeController : Controller
    {
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

        // ++++ Welcome Page Views - John Kirkvold ++++
        public ActionResult EquipmentView()
        {
            return View("EquipmentView");
        }

        public ActionResult SpaceView()
        {
            return View("SpaceView");
        }

        public ActionResult ConcessionsView()
        {
            return View("ConcessionsView");
        }
        // ++++ Welcome Page Views - John Kirkvold ++++

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

        // Go to Concessions Add item page
        public ActionResult ConcessionsAddItemView()
        {
            return View("ConcessionsAddItemView");
        }
        // --- Nate OMeara

        // ++ Added John Kirkvold
        // Go to Storage item page
        public ActionResult StorageView()
        {
            return View("StorageView");
        }
        // ++ Added John Kirkvold
    }
}