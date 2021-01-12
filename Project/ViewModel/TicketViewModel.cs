using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Models;

namespace Project.ViewModel
{
    public class TicketViewModel
    {
        public Ticket ticket { get; set; }
        public List<Ticket> tickets { get; set; }
    }
}