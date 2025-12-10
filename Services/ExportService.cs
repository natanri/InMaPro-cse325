// Services/ExportService.cs
using InMaPro_cse325.Models;
using System.Text;

namespace InMaPro_cse325.Services
{
    public interface IExportService
    {
        byte[] ExportInventoryToCsv(List<InventoryItem> items);
        string GetCsvFileName();
    }

    public class ExportService : IExportService
    {
        public byte[] ExportInventoryToCsv(List<InventoryItem> items)
        {
            var csv = new StringBuilder();
            
            // Encabezados
            csv.AppendLine("ID,Nombre,Descripción,Cantidad,Precio,Categoría,Fecha Creación,Última Actualización,Estado");
            
            // Datos
            foreach (var item in items)
            {
                var status = GetStockStatus(item.Quantity);
                
                csv.AppendLine(
                    $"{item.Id}," +
                    $"\"{EscapeCsvField(item.Name)}\"," +
                    $"\"{EscapeCsvField(item.Description)}\"," +
                    $"{item.Quantity}," +
                    $"{item.Price:F2}," +
                    $"\"{EscapeCsvField(item.Category?.Name ?? "Sin categoría")}\"," +
                    $"\"{item.CreatedAt:yyyy-MM-dd HH:mm:ss}\"," +
                    $"\"{(item.UpdatedAt.HasValue ? item.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "Nunca")}\"," +
                    $"\"{status}\""
                );
            }
            
            // Estadísticas al final
            csv.AppendLine();
            csv.AppendLine("ESTADÍSTICAS DEL REPORTE");
            csv.AppendLine($"Total Productos,{items.Count}");
            csv.AppendLine($"Total Unidades,{items.Sum(i => i.Quantity)}");
            csv.AppendLine($"Valor Total,{items.Sum(i => i.Quantity * i.Price):F2}");
            csv.AppendLine($"Productos Bajo Stock,{items.Count(i => i.Quantity < 10)}");
            csv.AppendLine($"Productos Agotados,{items.Count(i => i.Quantity == 0)}");
            csv.AppendLine($"Fecha Generación,\"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\"");
            csv.AppendLine($"Sistema,\"InMaPro Inventory Management\"");
            
            return Encoding.UTF8.GetBytes(csv.ToString());
        }
        
        public string GetCsvFileName()
        {
            return $"inventario_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        }
        
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";
            
            // Escapar comillas dobles
            return field.Replace("\"", "\"\"");
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