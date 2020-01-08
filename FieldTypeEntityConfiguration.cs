using Charts.Domain.Fields;
using Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charts.Infrastructure.EntityConfigurations.Charts
{
    public class FieldTypeEntityConfiguration : IEntityTypeConfiguration<FieldType>
    {
        public void Configure(EntityTypeBuilder<FieldType> builder)
        {
            builder.ToTable("FieldTypes", ChartContext.ChartSchema);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(ct => ct.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Ignore(a => a.SystemType);

            builder.HasData(Enumeration.GetAll<FieldType>());
        }
    }
}