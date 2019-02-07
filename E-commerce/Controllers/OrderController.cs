using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Database;
using MongoDB.Driver;
using MongoDB.Bson;

namespace E_commerce.Controllers
{
    [Authorize (Roles ="User")]
    public class OrderController : Controller
    {
        public ActionResult Checkout()
        {
            MongodbFunctions mongo = new MongodbFunctions();

            Database.DomainModel.CheckoutDetails details = new Database.DomainModel.CheckoutDetails();

            Database.DomainModel.User user= mongo.GetUser(User.Identity.Name);
            Database.DomainModel.Order order = mongo.GetOpenOrder(user.Id);
            details.User = user;
            List<Database.DomainModel.Product> products = new List<Database.DomainModel.Product>();

            foreach(MongoDBRef r in order.Products)
            {
                Database.DomainModel.Product product = mongo.GetProduct(new ObjectId(r.Id.ToString()));
                products.Add(product);
            }
            details.Products = products;

            return View(details);
        }

        [HttpPost]
        public void AddToChart(string id)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            Database.DomainModel.User user = mongo.GetUser(User.Identity.Name);
            Database.DomainModel.Order order = mongo.GetOpenOrder(user.Id);

            if(order==null)
            {
                List<MongoDBRef> products = new List<MongoDBRef>();
                products.Add(new MongoDBRef("products",new ObjectId(id)));

                order = new Database.DomainModel.Order
                {
                    Date=DateTime.Now,
                    Status="opened",
                    Products=products
                };

                mongo.AddUpdateOrder(order, user.Email, "add");
            }
            else
            {
                order.Products.Add(new MongoDBRef("products", new ObjectId(id)));
                mongo.AddUpdateOrder(order, user.Email, "update");
            }
        }

        [HttpPost]
        public JsonResult UpdateChart()
        {
            MongodbFunctions mongo = new MongodbFunctions();

            Database.DomainModel.User user = mongo.GetUser(User.Identity.Name);
            Database.DomainModel.Order order = mongo.GetOpenOrder(user.Id);//vraca opened order, samo 1 po useru moze da postoji

            int count;
            List<Database.DomainModel.ProductShow> products = new List<Database.DomainModel.ProductShow>();

            if (order == null)
                count = 0;
            else
            {
                count = order.Products.Count;

                foreach(MongoDBRef r in order.Products)
                {
                    Database.DomainModel.Product product = mongo.GetProduct(new ObjectId(r.Id.ToString()));

                    Database.DomainModel.ProductShow pr = new Database.DomainModel.ProductShow
                    {
                        Id = product.Id.ToString(),
                        Name=product.Name,
                        Price=product.Price
                    };

                    products.Add(pr);
                }
            }

            return Json(new {number=count,prod=products },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void DeleteFromChart(string id)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            Database.DomainModel.User user = mongo.GetUser(User.Identity.Name);
            Database.DomainModel.Order order = mongo.GetOpenOrder(user.Id);

            order.Products.Remove(new MongoDBRef("products", new ObjectId(id)));

            mongo.RemoveProduct(order);
        }

        [HttpPost]
        public void DeleteOrder()
        {
            MongodbFunctions mongo = new MongodbFunctions();

            Database.DomainModel.User user = mongo.GetUser(User.Identity.Name);
            Database.DomainModel.Order order = mongo.GetOpenOrder(user.Id);

            mongo.DeleteOrder(order.Id);
        }

        [HttpPost]
        public void SubmitOrder(string address, string note, string pay)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            Database.DomainModel.User user = mongo.GetUser(User.Identity.Name);
            Database.DomainModel.Order order = mongo.GetOpenOrder(user.Id);

            if(!user.Address.Contains(address))
            {
                user.Address.Add(address);
                mongo.UpdateAddresses(user);
            }

            order.Note = note;
            order.PayingMethod = pay;
            order.Address = address;

            mongo.CloseOrder(order);
        }
    }
}