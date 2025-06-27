#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RigidboysAPI.Models
{
    public class Product
    {   
        [Key]
        [Column("product_name")]
        public string Product_Name { get; set; } = string.Empty;

        [Column("category")]
        public string Category { get; set; } = string.Empty;

        [Column("license")]
        public string License { get; set; } = string.Empty;

        [Column("product_price")]
        public int? Product_price { get; set; }

        [Column("production_price")]
        public int? Production_price { get; set; }

        [Column("description")]
        public string? Description { get; set; }
    }
}
