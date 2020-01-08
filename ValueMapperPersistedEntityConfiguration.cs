using Charts.Domain.Fields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charts.Infrastructure.EntityConfigurations.Charts
{
    public class ValueMapperPersistedEntityConfiguration : IEntityTypeConfiguration<ValueMapperPersisted>
    {
        public void Configure(EntityTypeBuilder<ValueMapperPersisted> builder)
        {
            builder.ToTable("Mappers", ChartContext.ChartSchema);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .ForSqlServerUseSequenceHiLo("mapper", ChartContext.ChartSchema)
                .IsRequired();

            //builder.Property<int>("OriginFieldTypeId").IsRequired();

            builder.HasOne(o => o.OriginFieldType)
                .WithMany()
                .HasForeignKey("OriginFieldTypeId");
        }
    }
}
