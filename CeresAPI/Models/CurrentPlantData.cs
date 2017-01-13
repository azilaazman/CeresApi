using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CeresAPI.Models
{
    public class CurrentPlantData
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _id { get; set; }

        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId plant_id { get; set; }

        public string plant_name { get; set; }

        public string temp { get; set; }

        public string humid { get; set; }

        public string water { get; set; }

        public string care { get; set; }

        public string light { get; set; }

        public string power { get; set; }
    }

    public class RootObj
    {
        public List<CurrentPlantData> CurrentPlantData { get; set; }
    }
}