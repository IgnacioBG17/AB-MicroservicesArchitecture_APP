using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Web.Models.Coupon
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        [Required(ErrorMessage = "El Codigo de descuento es obligatorio")]
        public string CouponCode { get; set; }
        [Required(ErrorMessage = "El Importe de descuento es obligatorio")]
        public double DiscountAmount { get; set; }
        [Required(ErrorMessage = "El Cantidad mínima es obligatorio")]
        public int MinAmount { get; set; }
    }
}
