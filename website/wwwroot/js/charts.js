window.leafsteroidsCharts = (function () {
    var charts = {};

    function mount(id, option) {
        var el = document.getElementById(id);
        if (!el || !window.echarts) return;
        if (charts[id]) {
            charts[id].dispose();
        }
        var chart = echarts.init(el);
        chart.setOption(option);
        charts[id] = chart;
        requestAnimationFrame(function () { chart.resize(); });
    }

    function labels(rows) {
        return (rows || []).map(function (row) { return row.label; });
    }

    function values(rows) {
        return (rows || []).map(function (row) { return row.value; });
    }

    return {
        bars: function (id, rows, title) {
            var names = labels(rows);
            var rotate = names.some(function (label) { return String(label).length > 10; }) ? 24 : 0;
            mount(id, {
                title: { text: title, left: "center", textStyle: { fontSize: 16, color: "#023430" } },
                tooltip: { trigger: "axis" },
                grid: { left: 16, right: 16, bottom: rotate ? 4 : 12, top: 48, containLabel: true },
                xAxis: {
                    type: "category",
                    data: names,
                    axisLabel: { interval: 0, rotate: rotate, hideOverlap: false }
                },
                yAxis: { type: "value" },
                series: [{
                    type: "bar",
                    data: values(rows),
                    itemStyle: { color: "#00ed64" }
                }]
            });
        },
        line: function (id, rows, yName, title) {
            mount(id, {
                title: { text: title, left: "center", textStyle: { fontSize: 16, color: "#023430" } },
                tooltip: { trigger: "axis" },
                grid: { left: 16, right: 16, bottom: 8, top: 48, containLabel: true },
                xAxis: {
                    type: "category",
                    data: labels(rows),
                    axisLabel: { rotate: 40, hideOverlap: true }
                },
                yAxis: { type: "value", name: yName },
                series: [{
                    type: "line",
                    data: values(rows),
                    showSymbol: true,
                    color: "#196ee6"
                }]
            });
        },
        heatmap: function (id, points) {
            var data = (points || []).map(function (point) {
                return [point.x, point.z, point.count];
            });
            var max = data.reduce(function (highest, point) {
                return Math.max(highest, point[2]);
            }, 1);
            mount(id, {
                tooltip: {
                    trigger: "item",
                    formatter: function (item) {
                        var value = item.value;
                        return "X " + value[0] + "<br/>Z " + value[1] + "<br/>score " + value[2];
                    }
                },
                visualMap: {
                    min: 0,
                    max: max,
                    dimension: 2,
                    calculable: true,
                    orient: "vertical",
                    right: 0,
                    top: "center",
                    text: ["score", ""],
                    inRange: { color: ["#ffffb2", "#fd8d3c", "#bd0026"] }
                },
                grid: { left: 16, right: 78, bottom: 28, top: 28, containLabel: true },
                xAxis: { type: "value", name: "X", nameLocation: "middle", nameGap: 26, scale: true },
                yAxis: { type: "value", name: "Z", nameGap: 12, scale: true },
                series: [{
                    type: "scatter",
                    data: data,
                    symbolSize: function (value) {
                        return Math.max(8, Math.min(28, Math.sqrt(value[2]) * 3));
                    }
                }]
            });
        },
        resize: function () {
            Object.keys(charts).forEach(function (id) { charts[id].resize(); });
        }
    };
})();

window.addEventListener("resize", function () {
    if (window.leafsteroidsCharts) window.leafsteroidsCharts.resize();
});
