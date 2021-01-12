using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;
using Project.Dal;
using Project.ViewModel;

namespace Project.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RegisterUser()
        {
            return View(new User());
        }
        public ActionResult LoginUser()
        {
            return View(new User());
        }
        public ActionResult ChangeSeat()
        {
            TempData["ChangeSeat"] = Request.Form["MovieId"];
            int TicketId;
            Int32.TryParse(Request.Form["MovieId"], out TicketId);

            TicketDal dal = new TicketDal();
            List<Ticket> temp = (from x in dal.Tickets
                                 where x.id.Equals(TicketId) 
                                 select x).ToList<Ticket>();


            return View("SelectSeat",GetSeats(temp[0].MovieId));
        }
        public ActionResult SubChangeSeat()
        {
            int TicketId;
            Int32.TryParse(Request.Form["change"], out TicketId);
            string seat = Request.Form["seat"];
            TicketDal dal = new TicketDal();
            Ticket exist = (from x in dal.Tickets where x.id==TicketId select x).ToList<Ticket>()[0];
            string HallId = exist.HallId;
            string date = exist.date;
            string time = exist.time;
            List<Ticket> temp = (from x in dal.Tickets where  x.date.Equals(date) && x.time.Equals(time) &&
                                 x.HallId.Equals(HallId) && x.seat.Equals(seat) select x).ToList<Ticket>();

            if (temp.Count != 0)
            {
                TempData["testmsg"] = "Seat Is Occupied !!!";
                TempData["ChangeSeat"] = Request.Form["change"];
                return View("SelectSeat", GetSeats(exist.MovieId));
            }

            dal.Tickets.Remove(exist);
            dal.SaveChanges();
            exist.seat = seat;
            dal.Tickets.Add(exist);
            dal.SaveChanges();

            return RedirectToAction("ShoppingCart", "User");
        }
        public ActionResult SelectSeat()
        {
            int MovieId;
            Int32.TryParse(Request.Form["MovieId"], out MovieId);
            MovieDal temp = new MovieDal();
            Movie movie = (from x in temp.Movies where x.id.Equals(MovieId) select x).ToList<Movie>()[0];


            string MovieAge = movie.age;
            string email = (string)Session["email"];
            UserDal dal = new UserDal();
            List<User> UserExist = (from x in dal.Users where x.email.Equals(email) select x).ToList<User>();

            int IsGuest = 0;
            if (UserExist.Count == 0)
                IsGuest = 1;

            if (IsGuest == 0)
            {
                string UserAge= UserExist[0].age;
                int Mage, Uage;
                Int32.TryParse(MovieAge, out Mage);
                Int32.TryParse(UserAge, out Uage);
                if (Mage > Uage)
                {
                    TempData["CanBuy"] = "Your are under of the allowed age !!!";
                    return RedirectToAction("MovieGallery", "User");
                }
            }
            if (CanBuy(movie.date, movie.time) == 1)
            {
                int mid;
                Int32.TryParse(Request.Form["MovieId"], out mid);
                return View(GetSeats(mid));
            }

            TempData["CanBuy"] = "You can not buy a ticket for movie that started or ended !!!";
            return RedirectToAction("MovieGallery", "User");
            
        }
        public ActionResult Payment()
        {
            TempData["TicketId"] = Request.Form["TicketId"];
            return View();
        }
        public ActionResult ShoppingCart()
        {
            string email = (string)Session["email"];
            TicketDal dal = new TicketDal();
            TicketViewModel cvm = new TicketViewModel();
            List<Ticket> tickets = (from x in dal.Tickets where x.email.Equals(email) select x).ToList<Ticket>();
            cvm.ticket = new Ticket();
            cvm.tickets = tickets;
            return View(cvm);
        }
        
        public ActionResult SubFillter()
        {
            MovieDal dal = new MovieDal();
            MovieViewModel cvm = new MovieViewModel();
            List<Movie> movies = (from x in dal.Movies where x.preprice!=null select x).ToList<Movie>();
            cvm.movie = new Movie();
            cvm.movies = movies;
            return View("MovieGallery", cvm);
        }
        public ActionResult SubOrder()
        {
            MovieDal dal = new MovieDal();
            MovieViewModel cvm = new MovieViewModel();
            List<Movie> movies;
            switch ((string)Request.Form["states"])
            {
                case "Price increase":
                    movies = dal.Movies.OrderBy(c => c.price.Length).ThenBy(c => c.price).ToList<Movie>();
                    break;
                case "Price decrease":
                    movies = dal.Movies.OrderByDescending(x => x.price.Length).ThenByDescending(c => c.price).ToList<Movie>();
                    break;
                case "Most popular":
                    movies = SortByPopular(dal.Movies.ToList<Movie>());
                    break;
                default:
                    movies = dal.Movies.OrderBy(x => x.category).ToList<Movie>();
                    break;
            }
            cvm.movie = new Movie();
            cvm.movies = movies;
            return View("MovieGallery", cvm);
        }
        public ActionResult SubSearch()
        {
            MovieDal dal = new MovieDal();
            MovieViewModel cvm = new MovieViewModel();
            List<Movie> movies;
            string value1 = Request.Form["value1"];
            string value2 = Request.Form["value2"];
            int number2, number1;
            string date,time;
            switch ((string)Request.Form["Searchs"])
            {
                case "date":
                    movies = (from x in dal.Movies where x.date.Equals(value1) select x).ToList<Movie>();
                    break;
                case "time":
                    movies = (from x in dal.Movies where x.time.Equals(value1) select x).ToList<Movie>();
                    break;
                case "text":
                    movies = (from x in dal.Movies where x.category.Equals(value1) select x).ToList<Movie>();
                    break;
                case "datetime-local":
                    date = value1.Split('T')[0];
                    time = value1.Split('T')[1];
                    movies = (from x in dal.Movies where x.date.Equals(date) &&
                                                         x.time.Equals(time) select x).ToList<Movie>();
                    break;
                default:
                    Int32.TryParse(value1, out number1);
                    Int32.TryParse(value2, out number2);
                    movies = SeacgByRange(dal.Movies.ToList<Movie>(), number1,number2);
                    break;
            }
            cvm.movie = new Movie();
            cvm.movies = movies;
            return View("MovieGallery", cvm);
        }
        public ActionResult MovieGallery()
        {
            TicketDal temp = new TicketDal();
            MovieViewModel y = new MovieViewModel();
            List<Ticket> tickets = (from x in temp.Tickets where x.email.Equals("1") select x).ToList<Ticket>();
            if(tickets.Count == 0)
            {
                Ticket obj = new Ticket() { date = "None", time = "None", email = "1", HallId = "None", seat = "None" };
                temp.Tickets.Add(obj);
                temp.SaveChanges();
            }
            List<string> emails = (from x in temp.Tickets select x.email).ToList<string>();
            string key = MaxKey(emails);
            if(Session["email"] == null )
                 Session["email"] = key;

            MovieDal dal = new MovieDal();
            MovieViewModel cvm = new MovieViewModel();
            List<Movie> movies = dal.Movies.ToList<Movie>();
            cvm.movie = new Movie();
            cvm.movies = movies;
            return View(cvm);
        }
        public ActionResult SubLoginUser(User obj)
        {
            UserDal dal = new UserDal();
            string email = obj.email.ToString();
            string password = obj.password.ToString();
            List<User> exist = (from x in dal.Users where x.email.Equals(email) && x.password.Equals(password) select x).ToList<User>();
            if (exist.Count != 0)
            {
                Session["testmsg"] = exist[0].name;
                Session["email"] = exist[0].email;
                return RedirectToAction("MovieGallery", "User");
            }
            TempData["testmsg"] = "Some information is wrong please check again!!!";
            return View("LoginUser", obj);
        }
        public ActionResult SubRegisterUser(User obj)
        {
            UserDal dal = new UserDal();
            string key = obj.email.ToString();
            List<User> exist = (from x in dal.Users where x.email.Equals(key) select x).ToList<User>();
            if (exist.Count != 0)
            {
                TempData["testmsg"] = "This account is allready exist !!!";
                TempData["color"] = "red";
                return View("RegisterUser", obj);
            }

            dal.Users.Add(obj);
            dal.SaveChanges();
            TempData["testmsg"] = "Register Successfully !!!";
            TempData["color"] = "blue";
            return View("RegisterUser");
        }
        public ActionResult SubShoppingCart()
        {
            TicketDal dal = new TicketDal();
            string MovieName = (string)Session["MovieName"];
            int MovieId;
            Int32.TryParse(Request.Form["MovieId"], out MovieId);
            string date = Request.Form["date"];
            string time = Request.Form["time"];
            string HallId = Request.Form["HallId"];
            string price = Request.Form["price"];
            string seat = Request.Form["seat"];
            List<Ticket> exist = (from x in dal.Tickets where x.MovieName.Equals(MovieName)&&
                                  x.date.Equals(date) && x.time.Equals(time)&&
                                  x.HallId.Equals(HallId) && x.seat.Equals(seat) select x).ToList<Ticket>();

            if (exist.Count == 0)
            {
                Ticket obj = new Ticket();
                obj.MovieName = MovieName;
                obj.date = date;
                obj.time = time;
                obj.HallId = HallId;
                obj.price = price;
                obj.seat = seat;
                obj.MovieId = MovieId;
                obj.email = (string)Session["email"];
                dal.Tickets.Add(obj);
                dal.SaveChanges();
                return RedirectToAction("ShoppingCart", "User");
            }

            TempData["testmsg"] = "Seat Is Occupied !!!";
            return View("SelectSeat",GetSeats(MovieId));
        }
        public HallViewModel GetSeats(int MovieId)
        {
            MovieDal temp = new MovieDal();
            Movie movie = (from x in temp.Movies where x.id.Equals(MovieId) select x).ToList<Movie>()[0];

            HallDal halldal = new HallDal();
            string num = (from x in halldal.Halls where x.HallId.Equals(movie.HallId) select x.NumSeats).ToList<string>()[0];

            TicketDal dal = new TicketDal();
            HallViewModel cvm = new HallViewModel();
            List<string> halls = (from x in dal.Tickets
                                  where x.HallId.Equals(movie.HallId) &&x.date.Equals(movie.date) && x.time.Equals(movie.time)
                                  select x.seat).ToList<string>();
            cvm.halls = halls;
            Session["MovieName"] = movie.MovieName;
            TempData["date"] = movie.date;
            TempData["time"] = movie.time;
            TempData["HallId"] = movie.HallId;
            TempData["price"] = movie.price;
            TempData["MovieId"] = movie.id;
            TempData["NumSeats"] = num;
            return cvm;
        }
        public TicketViewModel GetTickets(string key)
        {
            TicketDal temp = new TicketDal();
            List<Ticket> tickets = (from x in temp.Tickets where x.email.Equals(key) select x).ToList<Ticket>();
            TicketViewModel cvm = new TicketViewModel();
            cvm.tickets = tickets;
            return cvm;
        }
        public ActionResult PaymentSuccess()
        {
            int TicketId;
            TicketDal dal = new TicketDal();
            List<Ticket> exist;
            string email =(string) Session["email"];

            if (Request.Form["TicketId"] != "")
            {
                Int32.TryParse(Request.Form["TicketId"], out TicketId);
                exist = (from x in dal.Tickets where x.id.Equals(TicketId) select x).ToList<Ticket>();

            }
            else
                exist = (from x in dal.Tickets where x.email.Equals(email) select x).ToList<Ticket>();

            foreach (Ticket t in exist)
                t.paid = "yes";

            dal.SaveChanges();
            TempData["success"] = Request.Form["TicketId"];
            return RedirectToAction("MovieGallery", "User");
        }
        public string MaxKey (List<string> keys)
        {
            bool isParsable;
            int  number, max = 0;
            foreach (string numberStr in keys)
            {
                
                isParsable = Int32.TryParse(numberStr, out number);
                if (isParsable)
                    max = Math.Max(max, number);
            }
            max += 1;
            return max.ToString();
        }
        public int CanBuy(string date,string time)
        {
            DateTime myDate = DateTime.Parse(date + " " + time + ":00");
            DateTime nowDate = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyy HH:mm"));
            int result = DateTime.Compare(myDate, nowDate);
            return result;
        }
        public int GetPopular(string name)
        {
            TicketDal dal = new TicketDal();
            TicketViewModel cvm = new TicketViewModel();
            List<Ticket> tickets = (from x in dal.Tickets where x.MovieName.Equals(name) select x).ToList<Ticket>();
            return tickets.Count;
        }
        public List<Movie> SortByPopular(List<Movie> a)
        {
            int i, j, min;
            Movie temp;
            for (i = 0; i < a.Count-1; i++)
            {
                min = i;
                for (j = i + 1; j < a.Count; j++)
                    if (GetPopular(a[j].MovieName) >= GetPopular(a[min].MovieName))
                        min = j;
                temp = a[i];
                a[i] = a[min];
                a[min] = temp;
            }
            return a;
        }
        public List<Movie> SeacgByRange(List<Movie> arr,int a,int b)
        {
            List<Movie> temp = new List<Movie>();
            foreach (Movie m in arr)
            {
                int price = Int32.Parse(m.price);
                if (price >= a && price <= b)
                    temp.Add(m);
            }
            return temp;
        }
        public ActionResult PaymentBefore()
        {
            TicketDal dal = new TicketDal();
            string MovieName = (string)Session["MovieName"];
            int MovieId;
            Int32.TryParse(Request.Form["MovieId"], out MovieId);
            string date = Request.Form["date"];
            string time = Request.Form["time"];
            string HallId = Request.Form["HallId"];
            string price = Request.Form["price"];
            string seat = Request.Form["seat"];
            List<Ticket> exist = (from x in dal.Tickets
                                  where x.MovieName.Equals(MovieName) &&
            x.date.Equals(date) && x.time.Equals(time) &&
            x.HallId.Equals(HallId) && x.seat.Equals(seat)
                                  select x).ToList<Ticket>();

            if (exist.Count == 0)
            {
                Ticket obj = new Ticket();
                obj.MovieName = MovieName;
                obj.date = date;
                obj.time = time;
                obj.HallId = HallId;
                obj.price = price;
                obj.seat = seat;
                obj.MovieId = MovieId;
                obj.email = (string)Session["email"];
                dal.Tickets.Add(obj);
                dal.SaveChanges();
                List<Ticket> ts = (from x in dal.Tickets
                                      where x.MovieName.Equals(MovieName) &&
                x.date.Equals(date) && x.time.Equals(time) &&
                x.HallId.Equals(HallId) && x.seat.Equals(seat)
                                      select x).ToList<Ticket>();

                TempData["TicketId"] = ts[0].id.ToString();
                return View("Payment");
            }
            TempData["testmsg"] = "Seat Is Occupied !!!";
            return View("SelectSeat", GetSeats(MovieId));


        }

        
    }
    
}