using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CeresAPI.Models
{
    public static class Connection
    {
        public static IMongoDatabase getMLabConnection() {
            MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
            IMongoDatabase db = client.GetDatabase("ceres_unit1");
            return db;
        }
    }
}