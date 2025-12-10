// Services/ExportService.cs
using InMaPro_cse325.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InMaPro_cse325.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportInventoryToCsvAsync(IQueryable<InventoryItem> query);
        Task<byte[]> ExportLowStockToCsvAsync(IQueryable<InventoryItem> query);
        byte[] ExportDashboardToCsv(DashboardStats stats);
        string GenerateCsvFileName(string prefix = "inventario");
    }

    public class ExportService : IExportService
    {
        public async Task<byte[]> ExportInventoryToCsvAsync(IQueryable<InventoryItem> query)
        {
            var items = await query
                .Include(i => i.Category)
                .ToListAsync();
            
            var csv = new StringBuilder();
            
            // Encabezados en español
            csv.AppendLine("ID,Nombre,Descripción,Cantidad,Precio Unitario,Valor Total,Categoría,Estado,Fecha Creación,Última Actualización");
            
            // Datos
            foreach (var item in items)
            {
                var totalValue = item.Quantity * item.Price;
                var status = GetStockStatus(item.Quantity);
                
                csv.AppendLine(
                    $"{item.Id}," +
                    $"\"{EscapeCsvField(item.Name)}\"," +
                    $"\"{EscapeCsvField(item.Description)}\"," +
                    $"{item.Quantity}," +
                    $"{item.Price:F2}," +
                    $"{totalValue:F2}," +
                    $"\"{EscapeCsvField(item.Category?.Name ?? "Sin categoría")}\"," +
                    $"\"{status}\"," +
                    $"\"{item.CreatedAt:yyyy-MM-dd HH:mm:ss}\"," +
                    $"\"{(item.UpdatedAt.HasValue ? item.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "Nunca")}\""
                );
            }
            
            // Footer con estadísticas
            csv.AppendLine();
            csv.AppendLine($"TOTAL PRODUCTOS,{items.Count}");
            csv.AppendLine($"UNIDADES TOTALES,{items.Sum(i => i.Quantity)}");
            csv.AppendLine($"VALOR TOTAL DEL INVENTARIO,{items.Sum(i => i.Quantity * i.Price):F2}");
            csv.AppendLine($"PRODUCTOS BAJO STOCK,{items.Count(i => i.Quantity < 10)}");
            csv.AppendLine($"PRODUCTOS AGOTADOS,{items.Count(i => i.Quantity == 0)}");
            csv.AppendLine($"FECHA DE EXPORTACIÓN,\"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\"");
            csv.AppendLine($"GENERADO POR,\"Sistema InMaPro\"");
            
            return Encoding.UTF8.GetBytes(csv.ToString());
        }
        
        public async Task<byte[]> ExportLowStockToCsvAsync(IQueryable<InventoryItem> query)
        {
            var lowStockItems = await query
                .Include(i => i.Category)
                .Where(i => i.Quantity < 10)
                .OrderBy(i => i.Quantity)
                .ToListAsync();
            
            var csv = new StringBuilder();
            
            // Encabezados específicos para bajo stock
            csv.AppendLine("INFORME DE PRODUCTOS CON BAJO STOCK");
            csv.AppendLine($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}");
            csv.AppendLine();
            csv.AppendLine("ID,Nombre,Categoría,Stock Actual,Stock Mínimo Recomendado,Diferencia,Precio Unitario,Valor en Stock,Estado,Última Actualización");
            
            foreach (var item in lowStockItems)
            {
                var difference = 10 - item.Quantity; // Stock mínimo recomendado: 10
                var status = GetStockStatus(item.Quantity);
                var stockValue = item.Quantity * item.Price;
                
                csv.AppendLine(
                    $"{item.Id}," +
                    $"\"{EscapeCsvField(item.Name)}\"," +
                    $"\"{EscapeCsvField(item.Category?.Name ?? "Sin categoría")}\"," +
                    $"{item.Quantity}," +
                    $"10," +
                    $"{difference}," +
                    $"{item.Price:F2}," +
                    $"{stockValue:F2}," +
                    $"\"{status}\"," +
                    $"\"{(item.UpdatedAt.HasValue ? item.UpdatedAt.Value.ToString("dd/MM/yyyy") : "Nunca")}\""
                );
            }
            
            // Resumen
            csv.AppendLine();
            csv.AppendLine("RESUMEN:");
            csv.AppendLine($"Total productos con bajo stock,{lowStockItems.Count}");
            csv.AppendLine($"Productos agotados,{lowStockItems.Count(i => i.Quantity == 0)}");
            csv.AppendLine($"Productos críticos (menos de 5 unidades),{lowStockItems.Count(i => i.Quantity < 5)}");
            csv.AppendLine($"Valor total en riesgo,{lowStockItems.Sum(i => i.Quantity * i.Price):F2}");
            
            return Encoding.UTF8.GetBytes(csv.ToString());
        }
        
        public byte[] ExportDashboardToCsv(DashboardStats stats)
        {
            var csv = new StringBuilder();
            
            // Encabezado del reporte
            csv.AppendLine("REPORTE DE DASHBOARD - INMAPRO");
            csv.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
            csv.AppendLine();
            
            // Estadísticas principales
            csv.AppendLine("ESTADÍSTICAS PRINCIPALES");
            csv.AppendLine($"Total de unidades en inventario,{stats.TotalItems}");
            csv.AppendLine($"Productos con bajo stock,{stats.LowStockItems}");
            csv.AppendLine($"Categorías activas,{stats.TotalCategories}");
            csv.AppendLine($"Valor total del inventario,{stats.TotalValue:F2}");
            csv.AppendLine();
            
            // Distribución por categoría
            csv.AppendLine("DISTRIBUCIÓN POR CATEGORÍA");
            csv.AppendLine("Categoría,Cantidad de Productos,Porcentaje");
            
            foreach (var category in stats.Categories)
            {
                var productCount = category.InventoryItems?.Count ?? 0;
                var percentage = stats.TotalItems > 0 ? 
                    (productCount * 100.0 / stats.TotalItems) : 0;
                
                csv.AppendLine(
                    $"\"{EscapeCsvField(category.Name)}\"," +
                    $"{productCount}," +
                    $"{percentage:F2}%"
                );
            }
            csv.AppendLine();
            
            // Productos recientes
            csv.AppendLine("PRODUCTOS AGREGADOS RECIENTEMENTE");
            csv.AppendLine("Nombre,Categoría,Cantidad,Precio,Fecha de Creación");
            
            foreach (var item in stats.RecentItems)
            {
                csv.AppendLine(
                    $"\"{EscapeCsvField(item.Name)}\"," +
                    $"\"{EscapeCsvField(item.Category?.Name ?? "Sin categoría")}\"," +
                    $"{item.Quantity}," +
                    $"{item.Price:F2}," +
                    $"\"{item.CreatedAt:dd/MM/yyyy}\""
                );
            }
            
            return Encoding.UTF8.GetBytes(csv.ToString());
        }
        
        public string GenerateCsvFileName(string prefix = "inventario")
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return $"{prefix}_{timestamp}.csv";
        }
        
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";
            
            // Si el campo contiene comas, comillas o saltos de línea, encerrar en comillas
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                // Escapar comillas dobles
                field = field.Replace("\"", "\"\"");
                return $"\"{field}\"";
            }
            
            return field;
        }
        
        private string GetStockStatus(int quantity)
        {
            return quantity switch
            {
                0 => "AGOTADO",
                < 5 => "CRÍTICO",
                < 10 => "BAJO STOCK",
                < 20 => "STOCK MEDIO",
                _ => "STOCK ALTO"
            };
        }
    }
}