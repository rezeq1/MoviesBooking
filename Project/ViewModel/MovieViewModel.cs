using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Project.Models;

namespace Project.ViewModel
{
    public class MovieViewModel
    {
        public Movie movie { get; set; }
        public List<Movie>  movies { get; set; }
    }
}