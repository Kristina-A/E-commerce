using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database.DomainModel
{
    public class Product
    {
        public ObjectId Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public List<string> Characteristics { get; set; }
        public List<MongoDBRef> Messages { get; set; }
        public List<MongoDBRef> Reviews { get; set; }

        public Product()
        {
            Messages = new List<MongoDBRef>();
            Reviews = new List<MongoDBRef>();
            Characteristics = new List<string>();
        }
    }
}
