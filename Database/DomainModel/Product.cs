using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Database.DomainModel
{
    public class Product
    {
        public ObjectId Id { get; set; }
        public MongoDBRef Category { get; set; }
        public string Name { get; set; }
        public List<string> Characteristics { get; set; }

        [BsonIgnore]
        public List<MongoDBRef> Messages { get; set; }

        [BsonIgnore]
        public List<MongoDBRef> Reviews { get; set; }

        public Product()
        {
            Messages = new List<MongoDBRef>();
            Reviews = new List<MongoDBRef>();
            Characteristics = new List<string>();
        }
    }
}
