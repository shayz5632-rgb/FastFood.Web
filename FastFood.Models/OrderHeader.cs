using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }

        public double OrderTotalOriginal { get; set; }

        public double OrderTotal { get; set; }

        public DateTime PickupTime { get; set; }

        public string CouponCode { get; set; }

        public double CouponCodeDiscount { get; set; }

        public string Status { get; set; }

        public string PaymentStatus { get; set; }

        public string TransactionId { get; set; }

        public string PhoneNumber { get; set; }

        public string PickupName { get; set; }

        public string Comments { get; set; }
    }
}