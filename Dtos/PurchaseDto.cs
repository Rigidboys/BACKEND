#nullable enable
using System.ComponentModel.DataAnnotations;

namespace RigidboysAPI.Dtos
{
    public class PurchaseDto
    {
        [Required(ErrorMessage = "매출 / 매입 선택해주세요")]
        public string Purchase_or_Sale { get; set; } = string.Empty;
        public string? Seller_Name { get; set; }

        [Required(ErrorMessage = "고객사명을 입력해주세요!")]
        public string Customer_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "거래일을 입력해주세요!")]
        public DateTime? Purchased_Date { get; set; }

        [Required(ErrorMessage = "거래된 제품을 입력해주세요!")]
        public string Product_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "가격을 입력해주세요!")]
        public int? Purchase_Price { get; set; }

        [Required(ErrorMessage = "수량을 입력해주세요!")]
        public int? Purchase_Amount { get; set; }

        //납부를 시작한 날짜
        public DateTime? Payment_Period_Start { get; set; }

        //납부를 마친 날짜
        public DateTime? Payment_Period_End { get; set; }

        [Required(ErrorMessage = "납기일을 입력해주세요!")]
        public DateTime? Payment_Period_Deadline { get; set; }

        [Required(ErrorMessage = "납부의 유뮤를 입력해주세요!")]
        public Boolean? Is_Payment { get; set; }

        [Required(ErrorMessage = "납부된 금액을 입력해주세요")]
        public int? Paid_Payment { get; set; }

        public string? Description { get; set; }
    }
}
