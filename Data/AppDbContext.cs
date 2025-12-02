namespace InMaPro_cse325.Data;

using InMaPro_cse325.Models;
using Microsoft.EntityFrameworkCore;




public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<InventoryItem> InventoryItems { get; set; }
}
