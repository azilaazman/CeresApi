using CeresAPI.Models;
using Microsoft.AspNet.SignalR;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;

namespace CeresAPI
{
    public class RandomNumberGenerator
    {
        static Random rnd1 = new Random();
        static public int randomScalingFactor()
        {

            return rnd1.Next(100);
        }
        static public int randomColorFactor()
        {

            return rnd1.Next(255);
        }

        static public IEnumerable<GraphPlantData> dataTemp()
        {
            var toJson = new WebClient().DownloadString("http://localhost:52781/api/v1/GetAllPlantsValue/5846c5f5f36d282dbc87f8d4");
            string json = Convert.ToString(toJson);

            List<GraphPlantData> instance = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GraphPlantData>>(json).ToList();

            //List<Data> listData = instance.Cast<Data>().ToList();
            return instance;

        }
    }

    //Line Chart class
    public class LineChart
    {
        [JsonProperty("lineChartData")]
        private IEnumerable<GraphPlantData> lineChartData;

        [JsonProperty("lineChartArrayTemp")]
        private double[] lineChartArrayTemp;

        [JsonProperty("lineChartArrayHumid")]
        private double[] lineChartArrayHumid;

        [JsonProperty("lineChartArrayPower")]
        private double[] lineChartArrayPower;

        [JsonProperty("lineChartArrayWater")]
        private double[] lineChartArrayWater;

        [JsonProperty("lineChartArrayLight")]
        private double[] lineChartArrayLight;

        [JsonProperty("lineChartTimeArray")]
        private string[] lineChartTimeArray;

        public void SetLineChartData()
        {
            lineChartData = RandomNumberGenerator.dataTemp().ToList();

            int i = 0;
            int ct = lineChartData.Count();
            lineChartArrayTemp = new double[ct];
            lineChartArrayHumid = new double[ct];
            lineChartArrayLight = new double[ct];
            lineChartArrayPower = new double[ct];
            lineChartArrayWater = new double[ct];
            lineChartTimeArray = new string[ct];

            foreach (var item in lineChartData)
            {
                lineChartArrayTemp[i] = double.Parse(item.temp);
                lineChartArrayHumid[i] = double.Parse(item.humid);
                lineChartArrayLight[i] = double.Parse(item.light);
                lineChartArrayPower[i] = double.Parse(item.power);
                lineChartArrayWater[i] = double.Parse(item.water);

                lineChartTimeArray[i] = item._id.CreationTime.ToLocalTime().ToShortTimeString();
                i++;
            }


        }

    }

    //The Pie Chart Class    
    public class PieChart
    {
        [JsonProperty("value")]
        private int[] pieChartData;

        public void SetPieChartData()
        {
            pieChartData = new int[3];
            pieChartData[0] = RandomNumberGenerator.randomScalingFactor();
            pieChartData[1] = RandomNumberGenerator.randomScalingFactor();
            pieChartData[2] = RandomNumberGenerator.randomScalingFactor();

        }

    }

    public class ChartDataUpdate
    {
        // Singleton instance    
        private readonly static Lazy<ChartDataUpdate> _instance = new Lazy<ChartDataUpdate>(() => new ChartDataUpdate());
        // Send Data every 5 seconds    
        readonly int _updateInterval = 5000;
        //Timer Class    
        private Timer _timer;
        private volatile bool _sendingChartData = false;
        private readonly object _chartUpateLock = new object();
        LineChart lineChart = new LineChart();
        PieChart pieChart = new PieChart();

        private ChartDataUpdate()
        {
        }

        public static ChartDataUpdate Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        // Calling this method starts the Timer    
        public void GetChartData()
        {
            _timer = new Timer(ChartTimerCallBack, null, _updateInterval, _updateInterval);

        }
        private void ChartTimerCallBack(object state)
        {
            if (_sendingChartData)
            {
                return;
            }
            lock (_chartUpateLock)
            {
                if (!_sendingChartData)
                {
                    _sendingChartData = true;
                    SendChartData();
                    _sendingChartData = false;
                }
            }
        }

        private void SendChartData()
        {
            lineChart.SetLineChartData();
            pieChart.SetPieChartData();
            GetAllClients().All.UpdateChart(lineChart);

        }

        private static dynamic GetAllClients()
        {
            return GlobalHost.ConnectionManager.GetHubContext<ChartHub>().Clients;
        }
    }
}