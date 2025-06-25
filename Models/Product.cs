#nullable enable
using System.ComponentModel.DataAnnotations.Schema;

namespace RigidboysAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Column("Product_Name")]
        public string Product_Name { get; set; } = string.Empty;

        [Column("Category")]
        public string Category { get; set; } = string.Empty;

        [Column("License")]
        public string License { get; set; } = string.Empty;

        [Column("Product_price")]
        public int? Product_price { get; set; }

        [Column("Production_price")]
        public int? Production_price { get; set; }

        [Column("Description")]
        public string? Description { get; set; }
    }
}
