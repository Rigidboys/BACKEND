#nullable enable
using System.ComponentModel.DataAnnotations.Schema;

namespace RigidboysAPI.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [Column("seller_name")]
        public string Seller_Name { get; set; } = string.Empty;

        [Column("purchase_or_sale")]
        public string Purchase_or_Sale { get; set; } = string.Empty;

        [Column("customer_name")]
        public string Customer_Name { get; set; } = string.Empty;

        [Column("purchased_date")]
        public DateTime? Purchased_Date { get; set; }

        [Column("product_name")]
        public string Product_Name { get; set; } = string.Empty;

        [Column("purchase_amount")]
        public int? Purchase_Amount { get; set; }

        [Column("purchase_price")]
        public int? Purchase_Price { get; set; }

        [Column("payment_period_start")]
        public DateTime? Payment_Period_Start { get; set; }

        [Column("payment_period_end")]
        public DateTime? Payment_Period_End { get; set; }

        [Column("payment_period_deadline")]
        public DateTime? Payment_Period_Deadline { get; set; }

        [Column("is_payment")]
        public bool? Is_Payment { get; set; }

        [Column("paid_payment")]
        public int? Paid_Payment { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("createdbyuserId")]
        public int CreatedByUserId { get; set; }
    }
}
