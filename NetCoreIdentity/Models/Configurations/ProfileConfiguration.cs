﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCoreIdentity.Models.Entities;

namespace NetCoreIdentity.Models.Configurations
{
    public class ProfileConfiguration : BaseConfiguration<AppUserProfile>
    {
        public override void Configure(EntityTypeBuilder<AppUserProfile> builder)
        {
            base.Configure(builder);
        }
    }
}
