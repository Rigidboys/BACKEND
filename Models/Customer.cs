#nullable enable
using System.ComponentModel.DataAnnotations.Schema;

namespace RigidboysAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Column("office_name")]
        public string Office_Name { get; set; } = string.Empty;

        [Column("type")]
        public string Type { get; set; } = string.Empty;

        [Column("master_name")]
        public string Master_Name { get; set; } = string.Empty;

        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        [Column("address")]
        public string? Address { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("createdbyuserid")]
        public int CreatedByUserId { get; set; }
    }
}
