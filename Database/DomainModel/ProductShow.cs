using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database.DomainModel
{
    public class ProductShow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
