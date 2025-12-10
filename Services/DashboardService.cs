// Services/DashboardService.cs
using InMaPro_cse325.Data;
using InMaPro_cse325.Models;
using Microsoft.EntityFrameworkCore;

namespace InMaPro_cse325.Services
{
    public class DashboardService
    {
        private readonly AppDbContext _context;
        
        public DashboardService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            var stats = new DashboardStats();
            
            // Total de items (suma de cantidades) - CORREGIDO para SQLite
            stats.TotalItems = await _context.InventoryItems
                .SumAsync(i => (int?)i.Quantity) ?? 0;
            
            // Items con bajo stock
            stats.LowStockItems = await _context.InventoryItems
                .Where(i => i.Quantity < 10)
                .CountAsync();
            
            // Total de categorías
            stats.TotalCategories = await _context.Categories.CountAsync();
            
            // Valor total del inventario - CORRECCIÓN PARA SQLite
            // Traer los datos y calcular en memoria
            var items = await _context.InventoryItems.ToListAsync();
            stats.TotalValue = items.Sum(i => i.Quantity * i.Price);
            
            // Items recientes
            stats.RecentItems = await _context.InventoryItems
                .Include(i => i.Category)
                .OrderByDescending(i => i.CreatedAt)
                .Take(5)
                .ToListAsync();
            
            // Todas las categorías
            stats.Categories = await _context.Categories
                .Include(c => c.InventoryItems)
                .ToListAsync();
            
            return stats;
        }
    }
}