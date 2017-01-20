using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CeresAPI.Models
{
    public class CreateAccount
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [Required]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        public string salt { get; set; }

        [Required]
        public String unit_id { get; set; }
    }
}