// Agregar estas funciones al archivo fileDownload.js

// Inicializar gráfico Chart.js
function initializeChart(chartId, config, dotNetHelper) {
    const ctx = document.getElementById(chartId);
    if (!ctx) {
        console.error(`Canvas element with id ${chartId} not found`);
        return;
    }
    
    const chart = new Chart(ctx, {
        ...config,
        options: {
            ...config.options,
            onClick: (evt, elements) => {
                if (elements.length > 0) {
                    const element = elements[0];
                    dotNetHelper.invokeMethodAsync('OnChartClick', element.datasetIndex, element.index);
                }
            }
        }
    });
    
    // Guardar referencia al gráfico
    window[`chart_${chartId}`] = chart;
}

// Actualizar datos del gráfico
function updateChart(chartId, data) {
    const chart = window[`chart_${chartId}`];
    if (chart) {
        chart.data = data;
        chart.update();
    }
}

// Exportar gráfico como imagen
function exportChartAsImage(chartId, fileName) {
    const chart = window[`chart_${chartId}`];
    if (!chart) {
        console.error(`Chart with id ${chartId} not found`);
        return;
    }
    
    const link = document.createElement('a');
    link.download = fileName;
    link.href = chart.toBase64Image();
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

// Destruir gráfico
function destroyChart(chartId) {
    const chart = window[`chart_${chartId}`];
    if (chart) {
        chart.destroy();
        delete window[`chart_${chartId}`];
    }
}

// Hacer funciones disponibles globalmente
window.initializeChart = initializeChart;
window.updateChart = updateChart;
window.exportChartAsImage = exportChartAsImage;
window.destroyChart = destroyChart;