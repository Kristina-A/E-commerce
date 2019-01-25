using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database.DomainModel
{
    public class Ordered
    {
        public ObjectId Id { get; set; }
        public MongoDBRef Product { get; set; }
        public MongoDBRef Order { get; set; }
    }
}
