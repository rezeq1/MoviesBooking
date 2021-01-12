using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Admin
    {
        public string name { set; get; }
        [Key]
        public string email { set; get; }
        public string password { set; get; }
        public string age { set; get; }

    }
}