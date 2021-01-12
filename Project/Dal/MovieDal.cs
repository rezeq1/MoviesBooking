using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Project.Models;

namespace Project.Dal
{
    public class MovieDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>().ToTable("MovieTbl");
        }
        public DbSet<Movie> Movies { get; set; }
    }
}