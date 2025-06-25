using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class LoginAttempt
{
    [Key]
    public int Id { get; set; }

    [Column("UserId")]
    public string UserId { get; set; } = string.Empty;

    [Column("FailedAttempts")] //실패횟수
    public int FailedAttempts { get; set; }

    [Column("NLockedUntil")]
    public DateTime? LockedUntil { get; set; }

    [Column("LastAttemptAt")]
    public DateTime LastAttemptAt { get; set; } = DateTime.UtcNow;
}
