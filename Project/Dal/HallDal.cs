using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Project.Models;


namespace Project.Dal
{
    public class HallDal:DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hall>().ToTable("HallTbl");
        }
        public DbSet<Hall> Halls { get; set; }
    }
}