#nullable enable
using System.ComponentModel.DataAnnotations.Schema;

namespace RigidboysAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Column("username")]
        public string Name { get; set; } = String.Empty;

        [Column("user_id")]
        public string UserId { get; set; } = string.Empty;

        [Column("password")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        public string Role { get; set; } = "User";
    }
}