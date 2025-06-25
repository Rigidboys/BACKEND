using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required(ErrorMessage = "아이디를 입력해주세요.")]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "비밀번호를 입력해주세요.")]
    public string Password { get; set; } = string.Empty;
}
