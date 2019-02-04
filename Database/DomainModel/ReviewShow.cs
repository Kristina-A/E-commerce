﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database.DomainModel
{
    public class ReviewShow
    {
        public ObjectId Id { get; set; }
        public double Grade { get; set; }
        public string Comment { get; set; }
    }
}
