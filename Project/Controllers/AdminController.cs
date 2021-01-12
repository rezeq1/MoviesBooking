using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Project.Dal;
using Project.Controllers;
using System.IO;
namespace Project.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RegisterAdmin()
        {

            return View(new Admin() );
        }
        public ActionResult LoginAdmin()
        {
            return View(new Admin());
        }  
        public ActionResult ManagingPrices()
        {
            return View(new Movie());
        }
        public ActionResult ManagingSeats()
        {
            return View();
        }
        public ActionResult ManagingHalls()
        {
            return View(new Movie());
        }
        public ActionResult AddMovie()
        {
            return View(new Movie());
        }
        public ActionResult RemoveMovie()
        {
            return View(new Movie());
        }
        public ActionResult AddHall()
        {
            return View(new Hall());
        }
        public ActionResult SubAddHall(Hall obj)
        {
            HallDal dal = new HallDal();

            string key = obj.HallId.ToString();
            List<Hall> exist = (from x in dal.Halls where x.HallId.Equals(key) select x).ToList<Hall>();
            if (exist.Count != 0)
            {
                TempData["testmsg"] = "This hall is allready exist !!!";
                TempData["color"] = "red";
                return View("AddHall", obj);
            }

            dal.Halls.Add(obj) ;
            dal.SaveChanges();
            TempData["testmsg"] = "Hall Added Successfully !!!";
            TempData["color"] = "blue";
            return View("AddHall");
        }
        public ActionResult SubRegisterAdmin(Admin obj)
        {
            AdminDal dal = new AdminDal();
            string key = obj.email.ToString();
            List<Admin> exist = (from x in dal.Admins where x.email.Equals(key) select x).ToList<Admin>();
            if(exist.Count != 0)
            {
                TempData["testmsg"] = "This account is allready exist !!!";
                TempData["color"] = "red";
                return View("RegisterAdmin", obj);
            }
                
            dal.Admins.Add(obj);
            dal.SaveChanges();
            TempData["testmsg"] = "Register Successfully !!!";
            TempData["color"] = "blue";
            return View("RegisterAdmin");
        }
        public ActionResult SubLoginAdmin(Admin obj)
        {
            AdminDal dal = new AdminDal();
            string email = obj.email.ToString();
            string password = obj.password.ToString();
            List<Admin> exist = (from x in dal.Admins where x.email.Equals(email) && x.password.Equals(password)  select x).ToList<Admin>();
            if (exist.Count != 0)
                return RedirectToAction("AddMovie", "Admin");

            TempData["testmsg"] = "Some information is wrong please check again!!!";
            return View("LoginAdmin", obj);
        }
        
        public ActionResult SubMangingPrices(Movie obj)
        {
            MovieDal dal = new MovieDal();
            string name = obj.MovieName.ToString();
            string date = obj.date.ToString();
            string time = obj.time.ToString();
            List<Movie> exist = (from x in dal.Movies where x.MovieName.Equals(name) && x.date.Equals(date) && x.time.Equals(time) select x).ToList<Movie>();
            if (exist.Count == 0)
            {
                TempData["msg"] = "Movie doesn't exist !!!";
                TempData["color"] = "red";
                return View("ManagingPrices", obj);
            }

            string price=exist[0].price;
            exist[0].price = obj.price;
            exist[0].preprice = price;
            dal.SaveChanges();
            TempData["msg"] = "Movie price updated successfully !!!";
            TempData["color"] = "blue";
            return View("RemoveMovie");
        }
        public ActionResult SubManagingHalls(Movie obj)
        {
            TicketDal dal = new TicketDal();
            MovieDal Mdal = new MovieDal();
            string name = obj.MovieName.ToString();
            string date = obj.date.ToString();
            string time = obj.time.ToString();
            string New_hall = obj.HallId.ToString();
 
            List<Movie> Mexist= (from x in Mdal.Movies where x.MovieName.Equals(name) && x.date.Equals(date) && x.time.Equals(time) select x).ToList<Movie>();
            
            if (Mexist.Count == 0)
            {
                TempData["msg"] = "Movie doesn't exist !!!";
                TempData["color"] = "red";
                return View("ManagingHalls", obj);
            }

            string Old_hall = Mexist[0].HallId;
            List<Ticket> exist = (from x in dal.Tickets where x.MovieName.Equals(name) && x.date.Equals(date) && x.time.Equals(time) && x.HallId.Equals(Old_hall) select x).ToList<Ticket>();

            if (exist.Count != 0)
            {
                TempData["msg"] = "You cant change movie hall , Someone buy ticket for this movie !!!";
                TempData["color"] = "red";
                return View("ManagingHalls", obj);
            }

            HallDal halls = new HallDal();
            List<Hall> HallExist = (from x in halls.Halls where x.HallId.Equals(New_hall) select x).ToList<Hall>();
            if (HallExist.Count == 0)
            {
                TempData["msg"] = "Hall not exist !!!";
                TempData["color"] = "red";
                return View("ManagingHalls", obj);
            }

            List<Movie> NewExist = (from x in Mdal.Movies where  x.date.Equals(date) && x.time.Equals(time) && x.HallId.Equals(New_hall) select x).ToList<Movie>();
            if (NewExist.Count != 0)
            {
                TempData["msg"] = "There is a movie at the same time and the same hall !!!";
                TempData["color"] = "red";
                return View("ManagingHalls", obj);
            }
            Mdal.Movies.Remove(Mexist[0]);
            obj.poster = Mexist[0].poster;
            obj.age = Mexist[0].age;
            obj.price = Mexist[0].price;
            obj.preprice = Mexist[0].preprice;
            obj.category = Mexist[0].category;
            Mdal.Movies.Add(obj);
            Mdal.SaveChanges();

            List<Movie> ms = (from x in Mdal.Movies where x.MovieName.Equals(name) && x.date.Equals(date) && x.time.Equals(time) && x.HallId.Equals(New_hall) select x).ToList<Movie>();
            foreach (Ticket t in exist)
                t.MovieId = ms[0].id;
            dal.SaveChanges();

            TempData["msg"] = "Movie hall  updated successfully !!!";
            TempData["color"] = "blue";
            return View("ManagingHalls");
        }
        public ActionResult SubManagingSeats(Hall obj)
        {

            HallDal dal = new HallDal();
            List<Hall> HallExist = (from x in dal.Halls where x.HallId.Equals(obj.HallId) select x).ToList<Hall>();
            if (HallExist.Count == 0)
            {
                TempData["msg"] = "Hall not exist !!!";
                TempData["color"] = "red";
                return View("ManagingSeats", obj);
            }

            TicketDal Tickdal = new TicketDal();
            List<Ticket> exist = (from x in Tickdal.Tickets where x.HallId.Equals(obj.HallId) select x).ToList<Ticket>();
            if (exist.Count != 0)
            {
                TempData["msg"] = "You cant change number of hall's seats , Someone buy ticket in this hall !!!";
                TempData["color"] = "red";
                return View("ManagingSeats", obj);
            }

            HallExist[0].NumSeats = obj.NumSeats;
            dal.SaveChanges();

            TempData["msg"] = "Number of hall's seats  updated successfully !!!";
            TempData["color"] = "blue";
            return View("ManagingSeats");
        }
        public ActionResult SubRemoveMovie(Movie obj)
        {
            MovieDal dal = new MovieDal();
            TicketDal tdal = new TicketDal();

            string name = obj.MovieName.ToString();
            string date = obj.date.ToString();
            string time = obj.time.ToString();
            List<Movie> exist = (from x in dal.Movies where x.MovieName.Equals(name) && x.date.Equals(date) && x.time.Equals(time)  select x).ToList<Movie>();
            if (exist.Count == 0)
            {
                TempData["msg"] = "Movie doesn't exist !!!";
                TempData["color"] = "red";
                return View("RemoveMovie", obj);
            }
            string hid = exist[0].HallId;
            List<Ticket> Texist = (from x in tdal.Tickets where x.MovieName.Equals(name) && x.date.Equals(date) && x.time.Equals(time) && x.HallId.Equals(hid) select x).ToList<Ticket>();
            if (Texist.Count != 0)
            {
                TempData["msg"] = "You can't remove this movie , someone buy it!!!";
                TempData["color"] = "red";
                return View("RemoveMovie", obj);
            }
            dal.Movies.Remove(exist[0]);
            dal.SaveChanges();
            TempData["msg"] = "Movie Removed Successfully !!!";
            TempData["color"] = "blue";
            return View("RemoveMovie");
        }
        public ActionResult SubAddMovie(Movie obj, MovieImage temp)
        {
            MovieDal dal = new MovieDal();
            string Hallid = obj.HallId.ToString();
            string date = obj.date.ToString();
            string time = obj.time.ToString();
            List<Movie> exist = (from x in dal.Movies where x.HallId.Equals(Hallid) && x.date.Equals(date) && x.time.Equals(time) select x).ToList<Movie>();
            if (exist.Count != 0)
            {
                TempData["msg"] = "There is a movie at the same time and the same hall !!!";
                TempData["color"] = "red";
                return View("AddMovie", obj);
            }

            HallDal halls= new HallDal();
            List<Hall> HallExist = (from x in halls.Halls where x.HallId.Equals(obj.HallId) select x).ToList<Hall>();
            if (HallExist.Count == 0)
            {
                TempData["msg"] = "Hall not exist !!!";
                TempData["color"] = "red";
                return View("AddMovie", obj);
            }

            string filename = Path.GetFileNameWithoutExtension(temp.ImageBytes.FileName);
            string extension = Path.GetExtension(temp.ImageBytes.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            obj.poster = "~/Images/" + filename;
            filename = Path.Combine(Server.MapPath("~/Images/"), filename);
            temp.ImageBytes.SaveAs(filename);

            dal.Movies.Add(obj);
            dal.SaveChanges();
            TempData["msg"] = "Movie Added Successfully !!!";
            TempData["color"] = "blue";
            return View("AddMovie");
        }





 }
}