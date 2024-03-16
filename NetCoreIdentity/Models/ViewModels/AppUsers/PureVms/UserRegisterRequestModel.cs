using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Models.ViewModels.AppUsers.PureVms
{
    public class UserRegisterRequestModel
    {
        [Required(ErrorMessage ="{0} Girilmesi Zorunludur")]
        [Display (Name = "Kullanıcı İsmi")]
        public string UserName { get; set; }


        [EmailAddress(ErrorMessage ="Email Formatında Giriş Yapınız")]
        public string Email { get; set; }


        [Required(ErrorMessage ="{0} Girilmesi Zorunludur")]
        [Display(Name ="Şifre ")]
        [MinLength(3,ErrorMessage ="Minimum {0} Karakter Girilmesi Gereklidir")]
        public string Password { get; set; }


        [Compare("Password",ErrorMessage ="Parolalar Uyuşmuyor")]
        public string ConforimPassword { get; set; }
    }
}
