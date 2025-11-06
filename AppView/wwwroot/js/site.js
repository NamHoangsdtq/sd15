var ChartJS = {
    BarChart: function (idChartCanvas, labelsArray, datasArray, label) {
        new Chart($("#" + idChartCanvas), {
            type: 'bar',
            data: {
                labels: labelsArray,
                datasets: [{
                    backgroundColor: [
                        'rgba(255, 99, 132, 0.55)',
                        'rgba(255, 159, 64, 0.55)',
                        'rgba(255, 205, 86, 0.55)',
                        'rgba(75, 192, 192, 0.55)',
                        'rgba(54, 162, 235, 0.55)'
                    ],
                    borderColor: [
                        'rgb(255, 99, 132)',
                        'rgb(255, 159, 64)',
                        'rgb(255, 205, 86)',
                        'rgb(75, 192, 192)',
                        'rgb(54, 162, 235)'
                    ],
                    data:datasArray
                }]
            },
            options: {
                responsive: false,
                plugins: {
                    legend: {
                        display:false
                    },
                    title: {
                        display: true,
                        text: label,
                        font: {
                            size: 16
                        }
                    }
                },
                
            }
        });
    },
    LineChart: function (idChartCanvas, labelsArray, datasArray,label) {
        new Chart($("#" + idChartCanvas), {
            type: 'line',
            data: {
                labels: labelsArray,
                datasets: [{
                    backgroundColor: "rgb(75, 192, 192)",
                    borderColor: "rgb(75, 192, 192)",
                    data: datasArray
                }]
            },
            options: {
                responsive: false,
                plugins: {
                    legend: {
                        display: false
                    },
                    title: {
                        display: true,
                        text: label,
                        font: {
                            size: 16
                        }
                    }
                },
                scales: {
                    yAxes: [{ ticks: { min: 6, max: 16 } }],
                }
            }
        });
    },
    PieChart: function (idChartCanvas, labelsArray, datasArray, label) {
        new Chart($("#" + idChartCanvas), {
            type: 'pie',
            data: {
                labels: labelsArray,
                datasets: [{
                    backgroundColor: ['rgb(75, 192, 192)',
                        'rgb(255, 99, 132)'],
                    data: datasArray
                }]
            },
            options: {
                responsive: false,
                plugins: {
                    title: {
                        display: true,
                        text: label,
                        font: {
                            size: 16
                        }
                    }
                }
            }
        });
    }
}
