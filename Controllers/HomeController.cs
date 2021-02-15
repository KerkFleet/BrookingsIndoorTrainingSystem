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
            ViewBag.Message = "Choose your desired space below:";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Equipment Manager";

            return View();
        }
    }
}