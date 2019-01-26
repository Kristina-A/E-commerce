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
    public class User
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public List<string> Address { get; set; }
        public string Phone { get; set; }

        [BsonIgnore]
        public List<MongoDBRef> Orders { get; set; }

        [BsonIgnore]
        public List<MongoDBRef> Reviews { get; set; }

        [BsonIgnore]
        public List<MongoDBRef> Messages { get; set; }

        public User()
        {
            Orders = new List<MongoDBRef>();
            Reviews = new List<MongoDBRef>();
            Messages = new List<MongoDBRef>();
            Address = new List<string>();
        }
    }
}
