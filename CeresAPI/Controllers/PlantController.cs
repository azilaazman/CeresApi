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

namespace CeresAPI.Controllers
{
    public class plantController : ApiController
    {
        // GET api/v2/products
        [HttpGet]
        [Route("api/v1/getPlantInfo")]
        public async Task<string> GetPlantInfo()
        {
            // connect to mLab
            MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
            IMongoDatabase db = client.GetDatabase("ceres_unit1");
            List<Plant> plant_list = new List<Plant>();
            var plantInfo = db.GetCollection<Plant>("plant");

            var allPlants
                = await plantInfo.Find(new BsonDocument()).ToListAsync(); //get all documents in the collection
            //foreach (Plant plant in allPlants)
            //{
            //	plant_list.Add(plant);
            //}
            return allPlants.ToJson();
            //return plantInfo.ToJson();

        }

        [HttpGet]
        [Route("api/v1/getPlantData/{id}")]
        public async Task<string> GetPlantData(string id)
        {
            // connect to mLab
            MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
            IMongoDatabase db = client.GetDatabase("ceres_unit1");
            List<PlantData> plant_list = new List<PlantData>();
            var plantInfo = db.GetCollection<PlantData>("plantData");
            var filter = Builders<PlantData>.Filter.Eq("plant_id", id);
            await plantInfo.Find(filter)
                //=> is foreach 
                .ForEachAsync(data => plant_list.Add(data)); //get all documents in the collection
            //foreach (PlantData plant in allPlants)
            //{
            //    plant_list.Add(plant);
            //}
            return plant_list.ToJson();
            //return plantInfo.ToJson();

        }

        /*IN-PRODUCTION by Azila*/
        [HttpGet]
        [Route("api/v1/GetAllPlantsTemp/{id}")]
        public List<PlantData> getAllPlantsTemp(string id)
        {
            try
            {
                MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
                IMongoDatabase db = client.GetDatabase("ceres_unit1");

                List<PlantData> plantList = new List<PlantData>();
                var plantInfo = db.GetCollection<PlantData>("plantData");
               
                var condition = Builders<PlantData>.Filter.Eq("plant_id", id);
                var fields = Builders<PlantData>.Projection.Include(p => p.temp).Include(a => a.plant_id);
                var results = plantInfo.Find(condition).Project<PlantData>(fields).ToList();

                return results;

            }

            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/v1/GetAllPlantsWater/{id}")]
        public List<PlantData> getAllPlantsWater(string id)
        {
            try
            {
                MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
                IMongoDatabase db = client.GetDatabase("ceres_unit1");

                List<PlantData> plantList = new List<PlantData>();
                var plantInfo = db.GetCollection<PlantData>("plantData");

                var condition = Builders<PlantData>.Filter.Eq("plant_id", id);
                var fields = Builders<PlantData>.Projection.Include(p => p.water).Include(a => a.plant_id);
                var results = plantInfo.Find(condition).Project<PlantData>(fields).ToList();

                return results;

            }

            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/v1/GetAllPlantsHumid/{id}")]
        public List<PlantData> getAllPlantsHumid(string id)
        {
            try
            {
                MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
                IMongoDatabase db = client.GetDatabase("ceres_unit1");

                List<PlantData> plantList = new List<PlantData>();
                var plantInfo = db.GetCollection<PlantData>("plantData");

                var condition = Builders<PlantData>.Filter.Eq("plant_id", id);
                var fields = Builders<PlantData>.Projection.Include(p => p.humid).Include(a => a.plant_id);
                var results = plantInfo.Find(condition).Project<PlantData>(fields).ToList();

                return results;

            }

            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/v1/GetAllPlantsLight/{id}")]
        public List<PlantData> getAllPlantsLight(string id)
        {
            try
            {
                MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
                IMongoDatabase db = client.GetDatabase("ceres_unit1");

                List<PlantData> plantList = new List<PlantData>();
                var plantInfo = db.GetCollection<PlantData>("plantData");

                var condition = Builders<PlantData>.Filter.Eq("plant_id", id);
                var fields = Builders<PlantData>.Projection.Include(p => p.light).Include(a => a.plant_id);
                var results = plantInfo.Find(condition).Project<PlantData>(fields).ToList();

                return results;

            }

            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/v1/GetAllPlantsPower/{id}")]
        public List<PlantData> getAllPlantsPower(string id)
        {
            try
            {
                MongoClient client = new MongoClient("mongodb://user:password@ds127428.mlab.com:27428/ceres_unit1");
                IMongoDatabase db = client.GetDatabase("ceres_unit1");

                List<PlantData> plantList = new List<PlantData>();
                var plantInfo = db.GetCollection<PlantData>("plantData");

                var condition = Builders<PlantData>.Filter.Eq("plant_id", id);
                var fields = Builders<PlantData>.Projection.Include(p => p.power).Include(a => a.plant_id);
                var results = plantInfo.Find(condition).Project<PlantData>(fields).ToList();

                return results;

            }

            catch
            {
                throw;
            }
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


    }
}

