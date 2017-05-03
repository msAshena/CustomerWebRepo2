using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerWeb.Models.JsonModel
{
    public class Details
    {
        public string property { get; set; }
        public string customer { get; set; }
        public string action { get; set; }
        public int value { get; set; }
        public string file { get; set; }
        public string hash { get; set; }
    }
}