using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CeresAPI.Models
{
    public class CurrentPlantData
    {
        [BsonId]
        public ObjectId _id { get; set; }

        public string unit_id { get; set; }

        public string name { get; set; }

        public double temp { get; set; }

        public double humid { get; set; }

        public double water { get; set; }

        public double light { get; set; }

        public int care { get; set; }

        public string startDate { get; set; }
        
        public string endDate { get; set; }

        
    }
}