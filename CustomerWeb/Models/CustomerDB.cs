namespace CustomerWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CustomerDB : DbContext
    {
        public CustomerDB()
            : base("name=CustomerDB")
        {
        }

        public virtual DbSet<CustomersInfo> CustomersInfoes { get; set; }
        public virtual DbSet<UploadedFile> UploadedFiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomersInfo>()
                .Property(e => e.hash)
                .IsFixedLength();

            modelBuilder.Entity<UploadedFile>()
                .Property(e => e.ServerFileName)
                .IsFixedLength();
        }
    }
}
