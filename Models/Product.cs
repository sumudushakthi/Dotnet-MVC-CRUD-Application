using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCrudApp.Models
{
    public class Product
    {
        [Key]
        [MaxLength(10)] // Adjust the length as needed
        public string Id { get; set; } = string.Empty; 
        [MaxLength(255)]// MySQL compatible string length
        public string Name { get; set; } = string.Empty; 
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
