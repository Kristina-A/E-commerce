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

            mongo.InsertProduct(newProduct);
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
    }
}