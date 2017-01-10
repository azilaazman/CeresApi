using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CeresAPI
{
	public class Plant
	{
        [BsonId]
        public ObjectId _id { get; set; }
        public String name { get; set; }
		public double temp { get; set; }
		public double light { get; set; }
		public double water { get; set; }
		public double humid { get; set; }
        public int care { get; set; }
		public DateTime timestamp { get; set; }

	}
}
