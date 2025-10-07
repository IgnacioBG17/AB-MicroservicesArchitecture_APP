using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Web.Models.AuthAPI
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
