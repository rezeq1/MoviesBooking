using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Project.Models;

namespace Project.Dal
{
    public class TicketDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<TicketDal>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ticket>().ToTable("TicketTbl");
        }
        public DbSet<Ticket> Tickets { get; set; }
    }
}