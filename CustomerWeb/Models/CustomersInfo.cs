namespace CustomerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomersInfo")]
    public partial class CustomersInfo
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string customer { get; set; }

        public int value { get; set; }

        [StringLength(50)]
        public string file { get; set; }

        [StringLength(50)]
        public string action { get; set; }

        [StringLength(50)]
        public string property { get; set; }

        public bool added { get; set; }

        [StringLength(50)]
        public string hash { get; set; }

        public string errors { get; set; }
    }
}
