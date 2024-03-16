using System.ComponentModel.DataAnnotations;

namespace NetCoreIdentity.Models.Admins.AppRoles.RequestModels
{
    public class CreateRoleRequestModel
    {
        [Required(ErrorMessage ="Rol ismi gereklidir")]
        public string RoleName { get; set; }
    }
}
