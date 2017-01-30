using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using CeresAPI.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http.Cors;

namespace CeresAPI.Controllers
{
    //[EnableCors(origins: "http://ceresmonitor.s3-website-ap-southeast-1.amazonaws.com", headers: "*", methods: "*")]
    public class plantController : ApiController
    {

        private static string GetUserIPAddress()
        {
            var context = System.Web.HttpContext.Current;
            string ip = String.Empty;

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
                ip = context.Request.UserHostAddress;

            if (ip == "::1")
                ip = "127.0.0.1";

            return ip;
        }
        [HttpGet]
        [Route("api/getip")]
        public string GetIp()
        {
            return GetUserIPAddress();
        }

        // GET api/v2/products
        [HttpGet]
        [Route("api/v1/getPlantInfo")]
        public async Task<string> GetPlantInfo()
        {
            // connect to mLab
            MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
            IMongoDatabase db = client.GetDatabase("ceres_unit1");
            List<CurrentPlantData> plant_list = new List<CurrentPlantData>();
            var plantInfo = db.GetCollection<CurrentPlantData>("plant");

            var allPlants
                = await plantInfo.Find(new BsonDocument()).ToListAsync(); //get all documents in the collection
            //foreach (Plant plant in allPlants)
            //{
            //	plant_list.Add(plant);
            //}
            return allPlants.ToJson();
            //return plantInfo.ToJson();

        }

        //[HttpGet]
        //[Route("api/v1/getPlantData/{id}")]
        //public async Task<string> GetPlantData(string id)
        //{
        //    // connect to mLab
        //    MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
        //    IMongoDatabase db = client.GetDatabase("ceres_unit1");
        //    List<PlantData> plant_list = new List<PlantData>();
        //    var plantInfo = db.GetCollection<PlantData>("plant");
        //    var filter = Builders<PlantData>.Filter.Eq("_id", id);
        //    await plantInfo.Find(filter)
        //        //=> is foreach 
        //        .ForEachAsync(data => plant_list.Add(data)); //get all documents in the collection
        //    //foreach (PlantData plant in allPlants)
        //    //{
        //    //    plant_list.Add(plant);
        //    //}
        //    return plant_list.ToJson();
        //    //return plantInfo.ToJson();

        //}

        [HttpGet]
        [Route("api/v1/GetAllPlantsValue/{id}")]
        public List<PlantData> getAllPlantsValue(string id)
        {
            try
            {
                MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
                IMongoDatabase db = client.GetDatabase("ceres_unit1");

                //List<PlantData> plantList = new List<PlantData>();
                var plantInfo = db.GetCollection<PlantData>("plantData");

                var condition = Builders<PlantData>.Filter.Eq("plant_id", id); //get requested unit/plant info
                //condition = condition & Builders<PlantData>.Filter.Eq("acc_id", acc); //makes sure that this belongs to the user

                //var fields = Builders<PlantData>.Projection.Include(a => a.plant_id);

                var results = plantInfo.Find(condition).ToList().OrderByDescending(p => p._id.CreationTime).Take(30).ToList();

                return results;

            }

            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("api/v1/GetLatestPlantValue/{id}")]
        public List<PlantData> getLatestPlantValue(string id)
        {
            try
            {
                MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
                IMongoDatabase db = client.GetDatabase("ceres_unit1");

                //List<PlantData> plantList = new List<PlantData>();
                var plantInfo = db.GetCollection<PlantData>("plantData");

                var condition = Builders<PlantData>.Filter.Eq("plant_id", id); //get requested unit/plant info
                //condition = condition & Builders<PlantData>.Filter.Eq("acc_id", acc); //makes sure that this belongs to the user

                //var fields = Builders<PlantData>.Projection.Include(a => a.plant_id);

                var results = plantInfo.Find(condition).ToList().OrderByDescending(p => p._id.CreationTime).Take(1).ToList();

                return results;

            }

            catch
            {
                throw;
            }
        }

        // GET api/v2/products
        [HttpPost]
        [Route("api/v1/getUnitSettings/{id}")]
        public List<CurrentPlantData> GetUnitSetting(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            // connect to mLab
            MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
            IMongoDatabase db = client.GetDatabase("ceres_unit1");
            //List<CurrentPlantData> plant_list = new List<CurrentPlantData>();
            var filter = Builders<CurrentPlantData>.Filter.Eq("_id", objId);
            var plantInfo = db.GetCollection<CurrentPlantData>("plant");

            var result = plantInfo.Find(filter).ToList(); 

            //await plantInfo.Find(filter)
            //     //=> is foreach 
            //     .ForEachAsync(data => plant_list.Add(data));
            //foreach (Plant plant in allPlants)
            //{
            // plant_list.Add(plant);
            //}
            return result;
            //return plantInfo.ToJson();

        }


        [HttpPost]
        [Route("api/v1/AddPlantData")]
        public async Task<string> AddPlantData([FromBody]PlantData plantData)
        {
            // connect to mLab
            MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
            IMongoDatabase db = client.GetDatabase("ceres_unit1");
            //List<PlantData> plant_list = new List<PlantData>();
            TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            Console.Write("timestamp "+span.ToString());
            
            var plantInfo = db.GetCollection<PlantData>("plantData");
            //var filter = Builders<PlantData>.Filter.Eq("plant_id", "jj");
            //await plantInfo.Find(filter)
            //=> is foreach
            //.ForEachAsync(data => plant_list.Add(data)); //get all documents in the collection
            //foreach (PlantData plant in allPlants)
            //{
            //    plant_list.Add(plant);
            //}
            if (ModelState.IsValid)
            {
                try
                {
                    await plantInfo.InsertOneAsync(plantData);
                    return plantData.ToJson();
                }
                catch (AggregateException aggEx)
                {
                    aggEx.Handle(x =>
                    {
                        var mwx = x as MongoWriteException;
                        if (mwx != null && mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                        {
                            // mwx.WriteError.Message contains the duplicate key error message
                            return true;
                        }
                        return false;
                    });
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway,ModelState).ToJson();
            }
           
            return HttpStatusCode.ExpectationFailed.ToString();
           //return plant_list.ToJson();
            //return plantInfo.ToJson();

        }

        [Route("api/v1/AddNewPlant")]
        [HttpPost]
        public string AddNewPlant(CurrentPlantData plant)
        {
            try
            {
                MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
                IMongoDatabase db = client.GetDatabase("ceres_unit1");

                var plantInfo = db.GetCollection<BsonDocument>("plant");


                BsonDocument plant_Doc = new BsonDocument
            {
                    {
                        "name", plant.name
                    },
                    {
                        "temp", plant.temp
                    },
                    {
                        "humid", plant.humid
                    },
                    {
                        "light", plant.light
                    },
                    {
                        "water", plant.water
                    },
                    {
                        "care", plant.care
                    },
                    {
                        "startDate", plant.startDate
                    },
                    {
                        "endDate", plant.endDate
                    }

            };
                plantInfo.InsertOne(plant_Doc);

                return "ok";
            }
            catch
            {
                throw;
            }
        }

        [Route("api/v1/UpdateUnitSettings/{id}")]
        [HttpPut]
        public async Task<string> UpdateUnitSettings(CurrentPlantData plant, string id)
        {
            ObjectId objId = ObjectId.Parse(id);

            try
            {
                MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
                IMongoDatabase db = client.GetDatabase("ceres_unit1");

                var plantColl = db.GetCollection<BsonDocument>("plant");



                BsonDocument plant_Doc = new BsonDocument
            {
                    {
                        "name", plant.name
                    },
                    {
                        "temp", plant.temp
                    },
                    {
                        "humid", plant.humid
                    },
                    {
                        "light", plant.light
                    },
                    {
                        "water", plant.water
                    },
                    {
                        "care", plant.care
                    },
                    {
                        "startDate", plant.startDate
                    },
                    {
                        "endDate", plant.endDate
                    }

            };

                var result = await plantColl.ReplaceOneAsync(
                    filter: new BsonDocument("_id", objId),
                    options: new UpdateOptions { IsUpsert = true },
                    replacement: plant_Doc);





                //plantColl.InsertOne(plant_Doc);

                return "ok";
        }
            catch
            {
                throw;
            }
        }


    }
}

