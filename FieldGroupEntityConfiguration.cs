using Charts.Domain.Fields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charts.Infrastructure.EntityConfigurations.Charts
{
    public class FieldGroupEntityConfiguration : IEntityTypeConfiguration<FieldGroup>
    {
        public void Configure(EntityTypeBuilder<FieldGroup> builder)
        {
            builder.ToTable("FieldGroups", ChartContext.ChartSchema);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .ForSqlServerUseSequenceHiLo("fieldgroup", ChartContext.ChartSchema)
                .IsRequired();

            builder.HasMany(a => a.Fields)
                .WithOne()
                .IsRequired();
        }
    }
}
