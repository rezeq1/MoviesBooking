using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Models;
using System.Data.Entity;

namespace Project.Dal
{
    public class AdminDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Admin>().ToTable("AdminTbl");
        }
        public DbSet<Admin> Admins { get; set; }
    }
}