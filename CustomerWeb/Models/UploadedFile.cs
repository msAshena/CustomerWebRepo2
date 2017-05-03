namespace CustomerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UploadedFile
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string FileName { get; set; }

        [StringLength(40)]
        public string ServerFileName { get; set; }
    }
}
