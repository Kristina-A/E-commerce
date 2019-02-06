using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Database;
using Database.DomainModel;

namespace E_commerce.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MongodbFunctions mongo = new MongodbFunctions();

            var randomproducts = mongo.GetProducts();

            return View(randomproducts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}