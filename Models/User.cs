#nullable enable
using System.ComponentModel.DataAnnotations.Schema;

namespace RigidboysAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; } = String.Empty;

        [Column("UserId")]
        public string UserId { get; set; } = string.Empty;

        [Column("PasswordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Column("Phone")]
        public string Phone { get; set; } = string.Empty;

        public string Role { get; set; } = "User";
    }
}