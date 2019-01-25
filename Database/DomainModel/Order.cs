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
        public string Note { get; set; }//napomena
        public string PayingMethod { get; set; }
        public MongoDBRef User { get; set; }
    }
}
