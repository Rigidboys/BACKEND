#nullable enable
using System.ComponentModel.DataAnnotations;

namespace RigidboysAPI.Dtos
{
    public class CustomerDto
    {
        [Required(ErrorMessage ="고객사명을 입력해주세요!")]
        public string OfficeName { get; set; } = string.Empty;

        [Required(ErrorMessage ="유형을 입력해주세요!")]
        public string Type { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "담당자명을 입력해주세요!")]
        public string Master_name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "연락처를 입력해주세요!")]
        public string Phone { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Description { get; set; }
    }
}