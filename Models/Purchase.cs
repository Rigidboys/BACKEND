#nullable enable
using System.ComponentModel.DataAnnotations.Schema;

namespace RigidboysAPI.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [Column("Seller_Name")]
        public string Seller_Name { get; set; } = string.Empty;

        [Column("Purchase_or_Sale")]
        public string Purchase_or_Sale { get; set; } = string.Empty;

        [Column("Customer_Name")]
        public string Customer_Name { get; set; } = string.Empty;

        [Column("Purchased_Date")]
        public DateTime? Purchased_Date { get; set; }

        [Column("Product_Name")]
        public string Product_Name { get; set; } = string.Empty;

        [Column("Purchase_Amount")]
        public int? Purchase_Amount { get; set; }

        [Column("Purchase_Price")]
        public int? Purchase_Price { get; set; }

        [Column("Payment_Period_Start")]
        public DateTime? Payment_Period_Start { get; set; }

        [Column("Payment_Period_End")]
        public DateTime? Payment_Period_End { get; set; }

        [Column("Payment_Period_Deadline")]
        public DateTime? Payment_Period_Deadline { get; set; }

        [Column("Is_Payment")]
        public bool? Is_Payment { get; set; }

        [Column("Paid_Payment")]
        public int? Paid_Payment { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("CreatedByUserId")]
        public int CreatedByUserId { get; set; }
    }
}
