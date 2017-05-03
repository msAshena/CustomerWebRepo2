using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerWeb.Models.ViewModel
{
    public class UploadResults
    {
        public string FileName { get; set; }
        public int RecordsCount { get; set; }
        public int SuccessCount { get; set; }
        public int FaildCount { get; set; }
    }
}