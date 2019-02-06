using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database.DomainModel
{
    public class Order
    {
        public ObjectId Id { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public string PayingMethod { get; set; }
        public string Status { get; set; }
        public MongoDBRef User { get; set; }
        public List<MongoDBRef> Products { get; set; }

        public Order()
        {
            Products = new List<MongoDBRef>();
        }
    }
}
