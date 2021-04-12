using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrookingsIndoorTrainingSystem.Models
{
    public class ConcessionsModel
    {
        public string itemName { get; set; }
        public int itemAmount { get; set; }
        public string itemLoc { get; set; }

    }

    //public class ConcessionsMakeSaleModel
    //{
    //    public List<ConcessionsModel> ConcessionsList { get; set; }
    //    public List<ConcessionsMakeSaleCartModel> ConcessionsCartList { get; set; }
    //}


    //public class ConcessionsMakeSaleCartModel
    //{
    //    public string itemName { get; set; }
    //    public int itemAmount { get; set; }
    //    public string itemLoc { get; set; }
    //}

    public static class GlobalConcessionsCartModel
    {
        public static List<ConcessionsModel> cart { get; set; }
    }

}