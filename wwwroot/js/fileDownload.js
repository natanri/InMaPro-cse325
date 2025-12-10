// wwwroot/js/downloadHelper.js

// Función para descargar archivos
function downloadFileFromBytes(fileName, byteArray, contentType) {
    try {
        // Convertir bytes a Blob
        const blob = new Blob([byteArray], { type: contentType });
        
        // Crear URL para el Blob
        const url = window.URL.createObjectURL(blob);
        
        // Crear link temporal
        const link = document.createElement('a');
        link.href = url;
        link.download = fileName;
        link.style.display = 'none';
        
        // Agregar al DOM y hacer clic
        document.body.appendChild(link);
        link.click();
        
        // Limpiar
        setTimeout(() => {
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);
        }, 100);
        
        console.log(`Archivo ${fileName} descargado exitosamente`);
        return true;
    } catch (error) {
        console.error('Error descargando archivo:', error);
        return false;
    }
}

// Función para mostrar toast notifications
function showToast(type, title, message) {
    // Crear contenedor si no existe
    let container = document.getElementById('toast-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'toast-container';
        container.className = 'toast-container position-fixed top-0 end-0 p-3';
        container.style.zIndex = '1060';
        document.body.appendChild(container);
    }
    
    // Crear toast
    const toastId = 'toast-' + Date.now();
    const toastHtml = `
        <div id="${toastId}" class="toast align-items-center text-bg-${type}" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    <strong>${title}</strong><br>
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;
    
    container.insertAdjacentHTML('beforeend', toastHtml);
    
    // Mostrar toast con Bootstrap
    const toastElement = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastElement, {
        autohide: true,
        delay: 5000
    });
    
    toast.show();
    
    // Remover después de ocultarse
    toastElement.addEventListener('hidden.bs.toast', function () {
        this.remove();
    });
}

// Hacer funciones disponibles globalmente
window.downloadFileFromBytes = downloadFileFromBytes;
window.showToast = showToast;