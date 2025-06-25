#nullable enable
using System.ComponentModel.DataAnnotations;

namespace RigidboysAPI.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = "이름을 입력해주세요")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "아이디를 입력해주세요")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "비밀번호를 입력해주세요")]
        [MinLength(4, ErrorMessage = "비밀번호는 최소 4자 이상이어야 합니다")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "이메일을 입력해주세요")]
        [EmailAddress(ErrorMessage = "이메일 형식이 올바르지 않습니다")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "전화번호를 입력해주세요")]
        [Phone(ErrorMessage = "전화번호 형식이 올바르지 않습니다")]
        public string Phone { get; set; } = string.Empty;
    }
}
