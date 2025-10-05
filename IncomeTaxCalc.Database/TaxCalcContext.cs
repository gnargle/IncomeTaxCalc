using IncomeTaxCalc.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Database
{
    public class TaxCalcContext : DbContext
    {
        public DbSet<Region> Regions { get; set; }
        public DbSet<TaxBand> TaxBands { get; set; }

        public TaxCalcContext(DbContextOptions<TaxCalcContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new RegionEntityTypeConfiguration().Configure(modelBuilder.Entity<Region>());
            new TaxBandEntityTypeConfiguration().Configure(modelBuilder.Entity<TaxBand>());
        }
    }
}
