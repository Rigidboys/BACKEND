using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LoginAttempt
{
    [Key]
    public int Id { get; set; }

    [Column("userId")]
    public string UserId { get; set; } = string.Empty;

    [Column("failedAttempts")] //실패횟수
    public int FailedAttempts { get; set; }

    [Column("lockedUntil")]
    public DateTime? LockedUntil { get; set; }

    [Column("lastAttemptAt")]
    public DateTime LastAttemptAt { get; set; } = DateTime.UtcNow;
}
