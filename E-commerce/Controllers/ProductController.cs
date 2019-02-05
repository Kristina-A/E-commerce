using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Database;
using MongoDB.Driver;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace E_commerce.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult NewProduct()
        {
            return View();
        }

        [HttpPost]
        public void AddNewProduct(string name, string category, string subcategory, int price, string characteristics)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            var picture = Request.Files["picture"];
            string path = System.IO.Path.Combine(Server.MapPath("~/Resources"), name + System.IO.Path.GetExtension(picture.FileName));
            picture.SaveAs(path);

            Database.DomainModel.Category cat = mongo.GetCategory(category);
            Database.DomainModel.Product newProduct = new Database.DomainModel.Product
            {
                Name=name,
                Price=price,
                Subcategory=subcategory,
                Picture= name + System.IO.Path.GetExtension(picture.FileName),
                Category=new MongoDBRef("categories",cat.Id),
                Characteristics = JsonConvert.DeserializeObject<List<string>>(characteristics)
            };

            mongo.InsertProduct(newProduct,category);
        }

        [HttpPost]
        public JsonResult ReturnSubcategories(string option)
        {
            MongodbFunctions mongo = new MongodbFunctions();
            List<string> subcategories=mongo.GetSubcategories(option);

            return Json(new { subcat = subcategories }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductDetails(string id)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            if (id.Equals(""))
                return RedirectToAction("Home", "Index");

            ObjectId objID = new ObjectId(id);

            Database.DomainModel.Product product = mongo.GetProduct(objID);
            if (product != null)
                return View(product);
            else
                return HttpNotFound();
        }

        [HttpPost]
        public void DeleteProduct(string id)
        {
            MongodbFunctions mongo = new MongodbFunctions();
            ObjectId objID = new ObjectId(id);
            mongo.DeleteProduct(objID);
        }

        [HttpPost]
        public void EditProduct(string id, string name, int price)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            ObjectId objID = new ObjectId(id);

            var picture = Request.Files["picture"];
            string path;
            if (picture != null)
            {
                string savePath = System.IO.Path.Combine(Server.MapPath("~/Resources"), name + System.IO.Path.GetExtension(picture.FileName));
                picture.SaveAs(savePath);
                path = name + System.IO.Path.GetExtension(picture.FileName);
            }
            else
            {
                Database.DomainModel.Product product = mongo.GetProduct(objID);
                path = product.Picture;
            }
            mongo.UpdateProduct(objID, name, price, path);
        }

        [HttpPost]
        public void EditCharacteristics(string id, string charName, string charValue, string oldN, string oldV)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            ObjectId objID = new ObjectId(id);
            Database.DomainModel.Product product = mongo.GetProduct(objID);

            List<string> chars = product.Characteristics;
            string newChar = charName + ":" + charValue;

            if (oldN.Equals("") || oldV.Equals(""))
            {
                chars.Add(newChar);
            }
            else
            {
                int index = chars.IndexOf(oldN + ":" + oldV);
                chars.RemoveAt(index);
                chars.Insert(index, newChar);
            }

            mongo.UpdateCharacteristics(objID, chars);
        }

        [HttpPost]
        public JsonResult AverageGrade(string id)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            ObjectId objID = new ObjectId(id);

            List<double> lista = mongo.AverageGrade(objID);

            return Json(new {number=lista[1],grade=lista[0] },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReviews(string id)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            ObjectId objID = new ObjectId(id);

            Database.DomainModel.Product product = mongo.GetProduct(objID);
            List<MongoDBRef> rev = product.Reviews;
            int count = product.Reviews.Count;

            List<Database.DomainModel.ReviewShow> reviews = new List<Database.DomainModel.ReviewShow>();
            List<Database.DomainModel.UserShow> users = new List<Database.DomainModel.UserShow>();

            foreach(MongoDBRef r in rev)
            {
                Database.DomainModel.Review review = mongo.GetReview(new ObjectId(r.Id.ToString()));
                Database.DomainModel.User user = mongo.GetUser(new ObjectId(review.User.Id.ToString()));

                Database.DomainModel.UserShow userShow = new Database.DomainModel.UserShow
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address
                };

                Database.DomainModel.ReviewShow reviewShow = new Database.DomainModel.ReviewShow
                {
                    Id=review.Id,
                    Grade=review.Grade,
                    Comment=review.Comment
                };
                reviews.Add(reviewShow);
                users.Add(userShow);
            }

            return Json(new { number = count, revs=reviews, people=users }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetComments(string id)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            ObjectId objID = new ObjectId(id);

            Database.DomainModel.Product product = mongo.GetProduct(objID);
            List<MongoDBRef> mess = product.Messages;
            int count = product.Messages.Count;
            string role;
            if (User.IsInRole("Admin"))
                role = "Admin";
            else
                role = "User";

            List<Database.DomainModel.MessageShow> comments = new List<Database.DomainModel.MessageShow>();
            List<Database.DomainModel.UserShow> users = new List<Database.DomainModel.UserShow>();

            foreach (MongoDBRef r in mess)
            {
                Database.DomainModel.Message comment = mongo.GetComment(new ObjectId(r.Id.ToString()));
                Database.DomainModel.User user = mongo.GetUser(new ObjectId(comment.User.Id.ToString()));
                List<string> responses = new List<string>();

                foreach (MongoDBRef rr in comment.Responses)
                {
                    Database.DomainModel.AdminResponse response = mongo.GetResponse(new ObjectId(rr.Id.ToString()));
                    responses.Add(response.Content);
                }

                Database.DomainModel.MessageShow messShow = new Database.DomainModel.MessageShow
                {
                    Id = comment.Id.ToString(),
                    Content=comment.Content,
                    Responses=responses
                };
                Database.DomainModel.UserShow userShow = new Database.DomainModel.UserShow
                {
                    Id=user.Id,
                    Name=user.Name,
                    Surname=user.Surname,
                    Email=user.Email,
                    Phone=user.Phone,
                    Address=user.Address
                };
                comments.Add(messShow);
                users.Add(userShow);
            }

            return Json(new { number = count, status=role, com = comments, people = users },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void AddComment(string prodId, string content)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            Database.DomainModel.Message newMessage = new Database.DomainModel.Message
            {
                Content=content,
                Product=new MongoDBRef("products",new ObjectId(prodId))
            };

            mongo.AddComment(newMessage, prodId, User.Identity.Name);
        }

        [HttpPost]
        public void AddReview(string id, double grade, string comment)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            Database.DomainModel.Review newReview = new Database.DomainModel.Review
            {
                Grade=grade,
                Comment=comment,
                Product=new MongoDBRef("products",new ObjectId(id))
            };

            mongo.AddReview(newReview, id, User.Identity.Name);
        }

        [HttpPost]
        public void AddResponse(string id, string content)
        {
            MongodbFunctions mongo = new MongodbFunctions();

            ObjectId messId = new ObjectId(id);

            Database.DomainModel.AdminResponse newResponse = new Database.DomainModel.AdminResponse
            {
                Content=content,
                Message=new MongoDBRef("messages",messId)
            };

            mongo.AddResponse(newResponse, messId);
        }
    }
}