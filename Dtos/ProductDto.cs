#nullable enable
using System.ComponentModel.DataAnnotations;

namespace RigidboysAPI.Dtos
{
    public class ProductDto
    {
        [Required(ErrorMessage = "제품명을 입력해주세요!")]
        public string Product_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "카테고리를 입력해주세요!")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "라이센스를 입력해주세요!")]
        public string License { get; set; } = string.Empty;

        [Required(ErrorMessage = "가격을 입력해주세요!")]
        public int? Product_price { get; set; }

        [Required(ErrorMessage = "실제 판매가를 입력해주세요.")]
        public int? Production_price { get; set; }
        public string? Description { get; set; }
    }
}
