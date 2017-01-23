using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using CeresAPI.Models;
using System.Collections.Generic;

namespace CeresAPI
{
	public class GraphPlantData
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
        public List<GraphPlantData> CurrentPlantData { get; set; }
    }


}
