using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IncomeTaxCalc.Database.Models
{
    public class TaxBand
    {
        public int TaxBandId { get; set; }
        public int RegionId { get; set; }
        public decimal LowerBound { get; set; }
        public decimal? UpperBound { get; set; }
        public decimal TaxRate { get; set; }
        public Region Region { get; set; }
    }

    public class TaxBandEntityTypeConfiguration : IEntityTypeConfiguration<TaxBand>
    {
        public void Configure(EntityTypeBuilder<TaxBand> builder)
        {
            builder.Property(b => b.LowerBound).HasColumnType("decimal(18, 4)");
            builder.Property(b => b.UpperBound).HasColumnType("decimal(18, 4)");
            builder.Property(b => b.TaxRate).HasColumnType("decimal(6, 6)");
        }
    }
}
