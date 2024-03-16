using NetCoreIdentity.Models.Admins.AppRoles.ResponseModels;

namespace NetCoreIdentity.Models.Admins.AppRoles.PageVms
{
    public class AssignRolePgeVM
    {
        public List<AppRoleResponseModel> Roles { get; set; }

        public int UserID { get; set; }
    }
}
