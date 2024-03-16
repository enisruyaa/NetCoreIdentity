using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Models.ViewModels.AppUsers.PureVms
{
    public class UserSignInRequestModel
    {
        [Required(ErrorMessage ="Kullanıcı İsmi Zorunludur")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Şifre Alanı Zorunludur")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; } // Bu alan kişinin başlangıçta gitmek istediği adresi tutar. Kişi eğer giriş yapmadan bir adrese gitmeye çalışırsa ve adres Authorization'a sahipse kişi engellenir Login'e atılır.. Loginden istenilen role sahip olduğunu kanıtlarsa tekrar ilk gitmek istediği adrese otomatik gönderilmesi kullanıcıyı daha çok tatmin edecği için açılmıştır. (Tamamıyla Kullanıcı dostu olmak için açılması zorunlu olan bir alan değil.)
    }
}
