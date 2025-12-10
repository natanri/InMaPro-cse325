// Models/Category.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InMaPro_cse325.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre de la categoría es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        // Relación inversa
        public virtual ICollection<InventoryItem>? InventoryItems { get; set; }
    }
}