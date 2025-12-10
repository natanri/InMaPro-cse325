// Models/DashboardStats.cs
namespace InMaPro_cse325.Models
{
    public class DashboardStats
    {
        public int TotalItems { get; set; }
        public int LowStockItems { get; set; }
        public int TotalCategories { get; set; }
        public decimal TotalValue { get; set; }
        public List<InventoryItem> RecentItems { get; set; } = new List<InventoryItem>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}