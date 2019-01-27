using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Database;
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
        public void AddNewProduct(string name, string category, string subcategory,string characteristics)
        {
            var picture = Request.Files["picture"];
            string path = System.IO.Path.Combine(Server.MapPath("~/Resources"), name + System.IO.Path.GetExtension(picture.FileName));
            picture.SaveAs(path);

            Database.DomainModel.Product newProduct = new Database.DomainModel.Product
            {
                Name=name,
                Characteristics= JsonConvert.DeserializeObject<List<string>>(characteristics)
            };
        }
    }
}