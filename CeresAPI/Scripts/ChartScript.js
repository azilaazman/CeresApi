
////////////////////////////////////////////////////////////////////////////////////////////////////////   
//Line Chart JSON Config (Line Chart Has fixed 1 data series here)    
var lineChartArray = {
    labels: [0],
    datasets: [
     {
         label: [0],
         fill: false,
         lineTension: 0.1,
         backgroundColor: "rgba(75,192,192,0.4)",
         borderColor: "rgba(75,192,192,1)",
         borderCapStyle: 'butt',
         borderDash: [],
         borderDashOffset: 0.0,
         borderJoinStyle: 'miter',
         pointBorderColor: "rgba(75,192,192,1)",
         pointBackgroundColor: "#fff",
         pointBorderWidth: 1,
         pointHoverRadius: 5,
         pointHoverBackgroundColor: "rgba(75,192,192,1)",
         pointHoverBorderColor: "rgba(220,220,220,1)",
         pointHoverBorderWidth: 2,
         pointRadius: 5,
         pointHitRadius: 10,
         data: [0]
     }
    ]

}

//Pie Chart JSON Config (Pie Chart Has fixed 3 Values/Slices here)    
var pieChartdata = [
             {
                 value: 0,
                 color: "#F7464A",
                 label: "East"
             },
             {
                 value: 0,
                 color: "#46BFBD",
                 label: "West"
             },
             {
                 value: 0,
                 color: "#FDB45C",
                 label: "North"
             },
             {
                 value: 0,
                 color: "#FE94DC",
                 label: "South"
             }
]

var pieData = {
    labels: [
        "Red",
        "Blue",
        "Yellow"
    ],
    datasets: [
        {
            data: [20, 30, 40],
            backgroundColor: [
                "#FF6384",
                "#36A2EB",
                "#FFCE56"],
            pointHoverBackgroundColor: [
                "#FF6384",
                "#36A2EB",
                "#FFCE56"]
        }
    ]
}

////////////////////////////////////////////////////////////////////////////////////////////////////////    

//PieChart Update method    
function UpdatePieChart(data) {
    //Set data returned from Server                
    //pieData[0].value = data.value[0];    
    //pieData[1].value = data.value[1];    
    //pieData[2].value = data.value[2];    
    //Update the Pie Chart    
    var canvasForPieChart = document.getElementById("canvasForPieChart");
    var context2DPie = canvasForPieChart.getContext("2d");
    //new Chart(context2DPie).Pie(pieChartdata);    
    var newPie = new Chart(context2DPie, {
        type: "pie",
        data: pieData,
    });

}

//LineChart Update method    
function UpdateLineChart(data) {
    //Set data returned from Server    
    //lineChartArray.datasets[0].fillColor = data.colorString;    
    lineChartArray.datasets[0].data = data.lineChartArray;
    lineChartArray.labels = data.lineChartTimeArray;
    //Update the Line Chart    
    //var canvasForLineChart = document.getElementById("canvasForLineChart");
    //var context2DLine = canvasForLineChart.getContext("2d");
    //var realChart = new Chart(context2DLine).Line(lineChartArray);    
    var newChart = new Chart(context2DLine, {
        type: "line",
        data: lineChartArray
    });
}
