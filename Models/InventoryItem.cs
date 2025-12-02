namespace InMaPro_cse325.Models;

public class InventoryItem
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string Category { get; set; } = string.Empty;

    public decimal Cost { get; set; }
}
