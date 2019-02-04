using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database.DomainModel
{
    public class MessageShow
    {
        public ObjectId Id { get; set; }
        public string Content { get; set; }
    }
}
