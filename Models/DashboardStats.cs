using System.Collections.Generic;

namespace InMaPro_cse325.Models
{
    public class DashboardStats
    {
        public int TotalItems { get; set; }          // Suma de Quantity de todos los items
        public int LowStockItems { get; set; }       // Items con Quantity < 10
        public int TotalCategories { get; set; }     // Número de categorías
        public decimal TotalValue { get; set; }      // Suma de (Quantity * Price)
        public List<InventoryItem> RecentItems { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
    }
}