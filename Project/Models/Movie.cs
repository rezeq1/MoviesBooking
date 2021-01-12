using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Movie
    {

        public string MovieName { set; get; }
        [Key]
        [Column(Order = 1)]
        public string date { set; get; }
        [Key]
        [Column(Order = 2)]
        public string time { set; get; }
        [Key]
        [Column(Order = 3)]
        public string HallId { set; get; }
        public string poster { set; get; }
        public string price { set; get; }
        public string preprice { set; get; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { set; get; }
        public string age { set; get; }
        public string category { set; get; }
    }
}