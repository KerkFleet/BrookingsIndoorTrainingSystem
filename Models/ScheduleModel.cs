using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrookingsIndoorTrainingSystem.Models
{
    public class ScheduleModel
    {
       
        public int Id { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }

        public string Thursday { get; set; }

        public string Friday { get; set; }

        public string Saturday { get; set; }

        public string Dates { get; set; }

    }
}