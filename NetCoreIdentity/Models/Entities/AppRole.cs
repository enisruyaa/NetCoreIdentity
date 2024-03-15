using Microsoft.AspNetCore.Identity;
using NetCoreIdentity.Models.Enums;
using NetCoreIdentity.Models.Interfaces;
using System;

namespace NetCoreIdentity.Models.Entities;

public class AppRole : IdentityRole<int>, IEntity
{
    public AppRole()
    {
        CreatedDate = DateTime.Now;
        Status = DataStatus.Inserted;
    }

    public int ID { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public DataStatus Status { get; set; }

    // Relational Properties

    public virtual ICollection<AppUserRole> UserRoles { get; set; }
}
