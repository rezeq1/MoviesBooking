using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class Hall
    {
        [Key]
        public string HallId { set; get; }
        public string NumSeats { set; get; }
    }
}