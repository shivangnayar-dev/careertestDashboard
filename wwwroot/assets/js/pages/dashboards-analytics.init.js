/*
Template Name: Auxx - Admin & Dashboard Template
Author: Themesdesign
Version: 1.0.0
Website: https://themesdesign.in/
Contact: Themesdesign@gmail.com
File: dashboard analytics init Js File
*/

// rgb to hex convert
function rgbToHex(rgb) {
    // Extract RGB values using regular expressions
    const rgbValues = rgb.match(/\d+/g);

    if (rgbValues.length === 3) {
        var [r, g, b] = rgbValues.map(Number);
    }
    // Ensure the values are within the valid range (0-255)
    r = Math.max(0, Math.min(255, r));
    g = Math.max(0, Math.min(255, g));
    b = Math.max(0, Math.min(255, b));

    // Convert each component to its hexadecimal representation
    const rHex = r.toString(16).padStart(2, '0');
    const gHex = g.toString(16).padStart(2, '0');
    const bHex = b.toString(16).padStart(2, '0');

    // Combine the hexadecimal values with the "#" prefix
    const hexColor = `#${rHex}${gHex}${bHex}`;

    return hexColor.toUpperCase(); // Convert to uppercase for consistency
}

// common function to get charts colors from class
function getChartColorsArray(chartId) {
    const chartElement = document.getElementById(chartId);
    if (chartElement) {
        const colors = chartElement.dataset.chartColors;
        if (colors) {
            const parsedColors = JSON.parse(colors);
            const mappedColors = parsedColors.map((value) => {
                const newValue = value.replace(/\s/g, "");
                if (!newValue.includes("#")) {
                    const element = document.querySelector(newValue);
                    if (element) {
                        const styles = window.getComputedStyle(element);
                        const backgroundColor = styles.backgroundColor;
                        return backgroundColor || newValue;
                    } else {
                        const divElement = document.createElement('div');
                        divElement.className = newValue;
                        document.body.appendChild(divElement);

                        const styles = window.getComputedStyle(divElement);
                        const backgroundColor = styles.backgroundColor.includes("#") ? styles.backgroundColor : rgbToHex(styles.backgroundColor);
                        return backgroundColor || newValue;
                    }
                } else {
                    return newValue;
                }
            });
            return mappedColors;
        } else {
            console.warn(`chart-colors attribute not found on: ${chartId}`);
        }
    }
}

//sales Overview
var options = {
    series: [{
        name: "Response Times",
        data: [10, 41, 35, 51, 49, 62, 69, 91, 148]
    },
    {
        name: "Response Times",
        data: [28, 20, 26, 32, 40, 59, 55, 79, 93]
    }],
    chart: {
        height: 412,
        type: 'line',
        zoom: {
            enabled: false
        },
        margin: {
            left: 0,
            right: 0,
            top: 0,
            bottom: 0
        },
        toolbar: {
            show: false,
        },
    },
    stroke: {
        show: true,
        curve: 'smooth',
        lineCap: 'butt',
        width: 2,
        dashArray: 0,
    },
    dataLabels: {
        enabled: false
    },
    grid: {
        xaxis: {
            lines: {
                show: true
            }
        },
        yaxis: {
            lines: {
                show: true
            }
        },
    },
    colors: getChartColorsArray("salesOverview"),
    xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'],
    }
};

var chart = new ApexCharts(document.querySelector("#salesOverview"), options);
chart.render();

//Daily Visitors
var options = {
    series: [{
        name: 'Viewers',
        data: [20, 13, 19, 23, 29, 42, 33, 29, 37, 46, 40, 49, 33, 29, 37, 46, 40, 49, 33, 29, 37, 46, 40, 49, 33, 29, 37, 46, 40, 21]
    }
    ],
    chart: {
        height: 350,
        type: 'bar',
        toolbar: {
            show: false,
        }
    },
    plotOptions: {
        bar: {
            borderRadius: 10,
            dataLabels: {
                position: 'top', // top, center, bottom
            },
        }
    },
    responsive: [
        {
            breakpoint: 992,
            options: {
                plotOptions: {
                    bar: {
                        columnWidth: '20px',
                    }
                },
            }
        },
        {
            breakpoint: 600,
            options: {
                plotOptions: {
                    bar: {
                        columnWidth: '15px',
                    }
                },
            }
        }
    ],
    dataLabels: {
        enabled: true,
        offsetY: -20,
        style: {
            fontSize: '12px',
            colors: ["#304758"]
        }
    },
    xaxis: {
        position: 'bottom',
        axisBorder: {
            show: false
        },
        axisTicks: {
            show: false
        },
        crosshairs: {
            fill: {
                type: 'gradient',
                gradient: {
                    colorFrom: '#D8E3F0',
                    colorTo: '#BED1E6',
                    stops: [0, 100],
                    opacityFrom: 0.4,
                    opacityTo: 0.5,
                }
            }
        },
        tooltip: {
            enabled: true,
        }
    },
    yaxis: {
        axisBorder: {
            show: false
        },
        axisTicks: {
            show: false,
        },
        labels: {
            show: false,
        }
    },
    stroke: {
        show: true,
        width: 4,
        colors: ['transparent']
    },
    grid: {
        show: false,
        padding: {
            top: 0,
            right: 0,
            bottom: 0,
            left: -10
        },
    },
    colors: getChartColorsArray("dailyVisitors")
};

var chart = new ApexCharts(document.querySelector("#dailyVisitors"), options);
chart.render();

//Page Views
var options = {
    series: [{
        name: 'Sent',
        data: [
            14, 20, 10, 5, 11, 30, 24, 26, 33, 38, 34, 29, 43
        ]
    }],
    chart: {
        type: 'area',
        height: 100,
        sparkline: {
            enabled: true
        },
        zoom: {
            autoScaleYaxis: true
        }
    },
    fill: {
        type: 'gradient',
        gradient: {
            shadeIntensity: 1,
            inverseColors: false,
            opacityFrom: 0.5,
            opacityTo: 0,
            stops: [0, 90, 100]
        },
    },
    colors: getChartColorsArray("pageViews"),
    stroke: {
        width: 2,
        curve: 'smooth',
    },
    dataLabels: {
        enabled: false
    },
};
var chart = new ApexCharts(document.querySelector("#pageViews"), options);
chart.render();

//Daily Visit Insights
var options = {
    series: [
        {
            name: 'Male',
            data: [55, 41, 67, 43, 21, 33, 45, 32, 21, 33, 45, 59]
        },
        {
            name: 'Female',
            data: [55, 41, 67, 43, 21, 33, 45, 53, 41, 67, 22, 43]
        }
    ],
    annotations: {
        points: [{
            x: 'Bananas',
            seriesIndex: 0,
            label: {
                borderColor: getChartColorsArray("dailyVisitInsightsChart")[1],
                offsetY: 0,
                style: {
                    color: '#fff',
                    background: getChartColorsArray("dailyVisitInsightsChart")[1],
                },
            }
        }]
    },
    colors: getChartColorsArray("dailyVisitInsightsChart"),
    chart: {
        height: 238,
        type: 'bar',
        toolbar: {
            show: false,
        }
    },
    plotOptions: {
        bar: {
            borderRadius: 10,
            columnWidth: '70%',
        }
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        width: 1
    },
    xaxis: {
        labels: {
            rotate: -45
        },
        categories: ['Jan', 'Feb', 'Mar', 'Apr','May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        tickPlacement: 'on'
    },
    fill: {
        type: 'gradient',
        gradient: {
            shade: 'light',
            type: "horizontal",
            shadeIntensity: 0.25,
            gradientToColors: undefined,
            inverseColors: true,
            opacityFrom: 0.85,
            opacityTo: 0.85,
            stops: [50, 0, 100]
        },
    },
    grid: {
        show: false,
        xaxis: {
            lines: {
                show: false
            }
        },
        yaxis: {
            lines: {
                show: false
            }
        },
        padding: {
            top: -10,
            right: -10,
            left: -10
        }
    },
    yaxis: {
        show: false,
    }
};

var chart = new ApexCharts(document.querySelector("#dailyVisitInsightsChart"), options);
chart.render();

//Subscription Distribution
var options = {
    series: [44, 55, 41, 17, 15],
    labels: ['Beginner', 'Intermediate', 'Enterprise', 'VIP', 'Professional'],
    chart: {
        height: 270,
        type: 'donut',
    },
    plotOptions: {
        pie: {
            startAngle: -90,
            donut: {
                size: '75%'
            }
        }
    },
    dataLabels: {
        enabled: false
    },
    fill: {
        type: 'gradient',
    },
    colors: getChartColorsArray("subscriptionDistribution"),
    legend: {
        formatter: function (val, opts) {
            return val + " - " + opts.w.globals.series[opts.seriesIndex]
        }
    },
    legend: {
        position: 'bottom'
    }
};

var chart = new ApexCharts(document.querySelector("#subscriptionDistribution"), options);
chart.render();

// Line with Data Labels
var dataLabelOptions = {
    series: [
        {
            name: "Income - 2023",
            data: [28, 29, 33, 36, 32]
        },
        {
            name: "Expense - 2023",
            data: [20, 17, 21, 29, 23]
        }
    ],
    chart: {
        height: 235,
        type: 'line',
        dropShadow: {
            enabled: true,
            color: '#000',
            top: 18,
            left: 7,
            blur: 10,
            opacity: 0.2
        },
        toolbar: {
            show: false
        }
    },
    colors: getChartColorsArray("lineWithDataLabel"),
    dataLabels: {
        enabled: true,
    },
    stroke: {
        curve: 'smooth',
        size: 2,
    },
    markers: {
        size: 1
    },
    yaxis: {
        show: false
    },
    xaxis: {
        categories: ['Mar', 'Apr', 'May', 'Jun', 'Jul'],
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right',
        floating: true,
        offsetY: -25,
        offsetX: -5
    },
    grid: {
        xaxis: {
            lines: {
                show: false
            }
        },
        yaxis: {
            lines: {
                show: false
            }
        },
        padding: {
            top: -25,
        }
    },
};

var chart = new ApexCharts(document.querySelector("#lineWithDataLabel"), dataLabelOptions);
chart.render();