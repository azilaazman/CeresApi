using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace CeresAPI.Models
{
    public class PlantData
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [Required]
        public string plant_id { get; set; }
        [Required]
        public double temp { get; set; }
        [Required]
        public double light { get; set; }
        [Required]
        public double water { get; set; }
        [Required]
        public double humid { get; set; }
        [Required]
        public double power { get; set; }
        [Required]
        public DateTime timestamp { get; set; }
    }
}