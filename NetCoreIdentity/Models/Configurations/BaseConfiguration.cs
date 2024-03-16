using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCoreIdentity.Models.Interfaces;

namespace NetCoreIdentity.Models.Configurations
{
    public class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreatedDate).HasColumnName("YaratilmaTarihi");
            builder.Property(x => x.ModifedDate).HasColumnName("GuncellenmeTarihi");
            builder.Property(x => x.DeletedDate).HasColumnName("SilinmeTarihi");
            builder.Property(x => x.Status).HasColumnName("VeriDurumu");
        }
    }
}
