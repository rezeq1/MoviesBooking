using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Project.Models
{
    public class Ticket
    {
        public Ticket()
        {
            paid = "no";
        }
        public string MovieName { set; get; }
        public string email { set; get; }
        [Key]
        [Column(Order = 1)]
        public string date { set; get; }
        [Key]
        [Column(Order = 2)]
        public string time { set; get; }
        [Key]
        [Column(Order = 3)]
        public string HallId { set; get; }
        [Key]
        [Column(Order = 4)]
        public string seat { set; get; }
        public string price { set; get; }
        public string paid { set; get; }
        
        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }
        public int MovieId { set; get; }

    }
}