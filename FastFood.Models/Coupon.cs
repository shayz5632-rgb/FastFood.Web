using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CouponType { get; set; }

        public double Discount { get; set; }

        public double MinimumAmount { get; set; }

        public bool IsActive { get; set; }
    }
}