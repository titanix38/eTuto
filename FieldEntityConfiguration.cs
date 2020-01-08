using Charts.Domain.Fields;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charts.Infrastructure.EntityConfigurations.Charts
{
    public class FieldEntityConfiguration : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            builder.ToTable("Fields", ChartContext.ChartSchema);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .ForSqlServerUseSequenceHiLo("field", ChartContext.ChartSchema)
                .IsRequired();

            builder.OwnsOne(a => a.Name);  
            
            
            
        }
    }
}
