using Charts.Domain.ChartAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charts.Infrastructure.EntityConfigurations.Charts
{
    public class BaseChartEntityConfiguration : IEntityTypeConfiguration<BaseChart>
    {
        public void Configure(EntityTypeBuilder<BaseChart> builder)
        {
            builder.ToTable("Basecharts", ChartContext.ChartSchema);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                .ForSqlServerUseSequenceHiLo("basechart", ChartContext.ChartSchema)
                .IsRequired();

            builder.Ignore(a => a.Type);
            builder.HasDiscriminator<ChartType>("ChartType");
            builder.HasMany(a => a.FieldGroups)
                .WithOne()
                .HasForeignKey("ChartId")
                .IsRequired();
          
        }
    }
}
