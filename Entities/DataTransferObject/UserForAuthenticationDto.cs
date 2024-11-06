
using System.ComponentModel.DataAnnotations;


namespace Entities.DataTransferObject
{
    public record class UserForAuthenticationDto
    {
        //kullanıcın giriş yapma esnasında eksik bir bilgi ilettiğinde göndereceğimiz mesajları bu şekilde iletiyoruz.
        [Required(ErrorMessage ="Username is required")]
        public string? UserName {  get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }
    }
}
