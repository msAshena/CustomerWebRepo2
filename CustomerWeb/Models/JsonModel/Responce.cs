using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerWeb.Models.JsonModel
{
    public class Responce
    {
        public bool added { get; set; }
        public string hash { get; set; }
        public List<string> errors { get; set; }
    }
}