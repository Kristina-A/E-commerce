using Database.DomainModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace Database
{
    public class MongodbFunctions
    {
        MongoClient client;
        IMongoDatabase db;

        public MongodbFunctions()
        {

            client = new MongoClient("mongodb://localhost:27017");
            db = client.GetDatabase("webshopdb");

        }


        public void InsertUser(User user)
        {
            var usersCollection = db.GetCollection<User>("users");

            usersCollection.InsertOne(user);
        }

        public void InsertProduct(Product product)
        {
            var productsCollection = db.GetCollection<Product>("products");

            productsCollection.InsertOne(product);
        }

        public List<string> GetSubcategories (string category)
        {
            var categoriesCollection = db.GetCollection<Category>("categories");

            var filter = Builders<Category>.Filter.Eq("Name", category);
            var categories = categoriesCollection.Find(filter);

            Category cat = categories.First();

            return cat.Subcategories;
        }

        public Category GetCategory(string category)
        {
            var categoriesCollection = db.GetCollection<Category>("categories");

            var filter = Builders<Category>.Filter.Eq("Name", category);
            var categories = categoriesCollection.Find(filter);

            return categories.First();
        }

        public Product GetProduct(string name)
        {
            var productsCollesction = db.GetCollection<Product>("products");

            var filter = Builders<Product>.Filter.Eq("Name", name);
            var products = productsCollesction.Find(filter);

            return products.First();
        }

    }
}
