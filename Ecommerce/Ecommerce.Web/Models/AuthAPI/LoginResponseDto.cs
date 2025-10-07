namespace Ecommerce.Web.Models.AuthAPI
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
