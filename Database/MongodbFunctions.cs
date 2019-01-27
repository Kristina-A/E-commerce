using Database.DomainModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var usersCollection = db.GetCollection<BsonDocument>("users");

            usersCollection.InsertOne(user.ToBsonDocument());
        }

        public void InsertProduct(Product product)
        {
            var productsCollection = db.GetCollection<BsonDocument>("products");

            productsCollection.InsertOne(product.ToBsonDocument());
        }

    }
}
