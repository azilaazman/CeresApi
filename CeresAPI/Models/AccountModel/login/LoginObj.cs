using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CeresAPI.Models.AccountModel.login
{
    public class LoginObj
    {
        [BsonId]
        public ObjectId _id { get; set; }

        public string name { get; set; }

        public string unit_id { get; set; }
    }
}