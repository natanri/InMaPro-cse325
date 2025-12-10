// Models/InventoryItem.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InMaPro_cse325.Models
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad debe ser un número positivo")]
        public int Quantity { get; set; }  // ¡IMPORTANTE! Esta es la propiedad correcta
        
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un número positivo")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        
        public DateTime CreatedAt { get; set; }  // ¡IMPORTANTE! Nombre correcto
        public DateTime? UpdatedAt { get; set; } // ¡IMPORTANTE! Nombre correcto
    }
}