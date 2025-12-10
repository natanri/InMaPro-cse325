// Data/SeedData.cs
using InMaPro_cse325.Models;
using Microsoft.EntityFrameworkCore;

namespace InMaPro_cse325.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // Verificar si ya hay datos
            if (context.Categories.Any() || context.InventoryItems.Any())
            {
                return; // La BD ya tiene datos
            }
            
            // Crear categorÃ­as
            var categories = new[]
            {
                new Category { Name = "ElectrÃ³nica", Description = "Dispositivos electrÃ³nicos" },
                new Category { Name = "Ropa", Description = "Prendas de vestir" },
                new Category { Name = "Hogar", Description = "ArtÃ­culos para el hogar" },
                new Category { Name = "Deportes", Description = "Equipamiento deportivo" }
            };
            
            context.Categories.AddRange(categories);
            context.SaveChanges();
            
            // Crear items de inventario (Â¡CORREGIDO! Usar propiedades correctas)
            var inventoryItems = new[]
            {
                new InventoryItem 
                { 
                    Name = "Laptop", 
                    Description = "Laptop de 15 pulgadas", 
                    Quantity = 10,  // Usar Quantity, NO Quantity
                    Price = 999.99m,
                    CategoryId = categories[0].Id,
                    CreatedAt = DateTime.UtcNow
                },
                new InventoryItem 
                { 
                    Name = "Smartphone", 
                    Description = "TelÃ©fono inteligente", 
                    Quantity = 25,  // Usar Quantity
                    Price = 499.99m,
                    CategoryId = categories[0].Id,
                    CreatedAt = DateTime.UtcNow
                },
                new InventoryItem 
                { 
                    Name = "Camiseta", 
                    Description = "Camiseta de algodÃ³n", 
                    Quantity = 50,  // Usar Quantity
                    Price = 19.99m,
                    CategoryId = categories[1].Id,
                    CreatedAt = DateTime.UtcNow
                },
                new InventoryItem 
                { 
                    Name = "Silla de oficina", 
                    Description = "Silla ergonÃ³mica", 
                    Quantity = 15,  // Usar Quantity
                    Price = 149.99m,
                    CategoryId = categories[2].Id,
                    CreatedAt = DateTime.UtcNow
                },
                new InventoryItem 
                { 
                    Name = "BalÃ³n de fÃºtbol", 
                    Description = "BalÃ³n oficial tamaÃ±o 5", 
                    Quantity = 30,  // Usar Quantity
                    Price = 29.99m,
                    CategoryId = categories[3].Id,
                    CreatedAt = DateTime.UtcNow
                }
            };
            
            context.InventoryItems.AddRange(inventoryItems);
            context.SaveChanges();
        }
    }
}
