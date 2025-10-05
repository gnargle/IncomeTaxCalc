using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IncomeTaxCalc.Database.Models
{
    public class Region
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public List<TaxBand> TaxBands { get; set; }
    }

    public class RegionEntityTypeConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.HasMany(e => e.TaxBands).WithOne(e => e.Region).HasForeignKey(e => e.RegionId);
        }
    }
}
