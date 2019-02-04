﻿using Database.DomainModel;
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

        public void InsertProduct(Product product, string cat)
        {
            var productsCollection = db.GetCollection<Product>("products");
            var categoriesCollection = db.GetCollection<Category>("categories");
            var filter = Builders<Category>.Filter.Eq("Name", cat);

            productsCollection.InsertOne(product);

            Category category = GetCategory(cat);
            category.Products.Add(new MongoDBRef("products", product.Id));
            var update = Builders<Category>.Update.Set("Products", category.Products);

            categoriesCollection.UpdateOne(filter, update);
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

        public Product GetProduct(ObjectId id)
        {
            var productsCollection = db.GetCollection<Product>("products");

            var filter = Builders<Product>.Filter.Eq("_id", id);
            var products = productsCollection.Find(filter);

            return products.First();
        }

        public void DeleteProduct(ObjectId id)
        {
            var productsCollection = db.GetCollection<Product>("products");

            var filter = Builders<Product>.Filter.Eq("_id", id);
            productsCollection.DeleteOne(filter);
        }

        public void UpdateProduct(ObjectId id, string name, int price, string path)
        {
            var productsCollection = db.GetCollection<Product>("products");

            var filter = Builders<Product>.Filter.Eq("_id", id);
            var update = Builders<Product>.Update.Set("Name", name)
                                                .Set("Price", price)
                                                .Set("Picture", path);
            productsCollection.UpdateOne(filter, update);
        }

        public void UpdateCharacteristics(ObjectId id, List<string> characteristics)
        {
            var productsCollection = db.GetCollection<Product>("products");

            var filter = Builders<Product>.Filter.Eq("_id", id);
            var update = Builders<Product>.Update.Set("Characteristics", characteristics);
            productsCollection.UpdateOne(filter, update);
        }

        public Review GetReview(ObjectId id)
        {
            var reviewsCollection = db.GetCollection<Review>("reviews");

            var filter = Builders<Review>.Filter.Eq("_id", id);
            var reviews = reviewsCollection.Find(filter);

            return reviews.First();
        }

        public List<double> AverageGrade(ObjectId id)//id proizvoda za prosecnu ocenu
        {
            List<double> lista = new List<double>();
            var reviewsCollection = db.GetCollection<Review>("reviews");
            var filter = Builders<Review>.Filter.Eq("Product", new MongoDBRef("products",id));
            var res = reviewsCollection.Aggregate().Match(filter).Group(c => c.Product, g =>
                     new
                     {
                         AvgGrade = g.Average(p => p.Grade),
                         Number = g.Count()
                     }).ToList();

            if (res.Count == 0)
            {
                lista.Add(0.0);//prosecna ocena
                lista.Add(0.0);//br reviewa
                return lista;
            }
            else
            {
                lista.Add(res.Select(_ => _.AvgGrade).First());
                lista.Add(res.Select(_ => _.Number).First());
                return lista;
            }
        }

        public User GetUser(ObjectId id)
        {
            var usersCollection = db.GetCollection<User>("users");

            var filter = Builders<User>.Filter.Eq("_id", id);
            var users = usersCollection.Find(filter);

            return users.First();
        }

        public User GetUser(string email)
        {
            var usersCollection = db.GetCollection<User>("users");

            var filter = Builders<User>.Filter.Eq("Email", email);

            return usersCollection.Find(filter).First();
        }

        public Message GetComment(ObjectId id)
        {
            var commentsCollection = db.GetCollection<Message>("messages");

            var filter = Builders<Message>.Filter.Eq("_id", id);
            var comments = commentsCollection.Find(filter);

            return comments.First();
        }

        public void AddComment(Message message, string prodId, string email)
        {
            User user = GetUser(email);

            message.User = new MongoDBRef("users", user.Id);
            var commentsCollection = db.GetCollection<Message>("messages");
            var usersCollection = db.GetCollection<User>("users");
            var productsCollection = db.GetCollection<Product>("products");

            commentsCollection.InsertOne(message);

            user.Messages.Add(new MongoDBRef("messages",message.Id));
            Product prod = GetProduct(new ObjectId(prodId));
            prod.Messages.Add(new MongoDBRef("messages", message.Id));

            var update = Builders<User>.Update.Set("Messages", user.Messages);
            var filter = Builders<User>.Filter.Eq("Email", email);
            var update1 = Builders<Product>.Update.Set("Messages", prod.Messages);
            var filter1 = Builders<Product>.Filter.Eq("_id", new ObjectId(prodId));

            usersCollection.UpdateOne(filter, update);
            productsCollection.UpdateOne(filter1, update1);
        }

        public void AddReview(Review review, string prodId, string email)
        {
            User user = GetUser(email);

            review.User = new MongoDBRef("users", user.Id);
            var reviewsCollection = db.GetCollection<Review>("reviews");
            var usersCollection = db.GetCollection<User>("users");
            var productsCollection = db.GetCollection<Product>("products");

            reviewsCollection.InsertOne(review);

            user.Reviews.Add(new MongoDBRef("reviews", review.Id));
            Product prod = GetProduct(new ObjectId(prodId));
            prod.Reviews.Add(new MongoDBRef("reviews", review.Id));

            var update = Builders<User>.Update.Set("Reviews", user.Reviews);
            var filter = Builders<User>.Filter.Eq("Email", email);
            var update1 = Builders<Product>.Update.Set("Reviews", prod.Reviews);
            var filter1 = Builders<Product>.Filter.Eq("_id", new ObjectId(prodId));

            usersCollection.UpdateOne(filter, update);
            productsCollection.UpdateOne(filter1, update1);
        }
    }
}
