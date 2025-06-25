#nullable enable
using System.ComponentModel.DataAnnotations.Schema;

namespace RigidboysAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Column("Office_Name")]
        public string Office_Name { get; set; } = string.Empty;

        [Column("Type")]
        public string Type { get; set; } = string.Empty;

        [Column("Master_Name")]
        public string Master_Name { get; set; } = string.Empty;

        [Column("Phone")]
        public string Phone { get; set; } = string.Empty;

        [Column("Address")]
        public string? Address { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("CreatedByUserId")]
        public int CreatedByUserId { get; set; }
    }
}
