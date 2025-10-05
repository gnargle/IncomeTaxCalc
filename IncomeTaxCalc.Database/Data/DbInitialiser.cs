using IncomeTaxCalc.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Database.Data
{
    public static class DbInitialiser
    {
        public static void Initialise(TaxCalcContext context)
        {
            if (context == null)
                throw new ArgumentNullException("No DB context provided");

            if (context.Regions.Any())
                return;

            var regions = new List<Region>()
            {
                new Region() { RegionName = "United Kingdom" },
                new Region() { RegionName = "Ireland" },
                new Region() { RegionName = "France" }
            };

            context.Regions.AddRange(regions);
            context.SaveChanges();

            var taxBands = new List<TaxBand>()
            {
                new TaxBand() { RegionId = (int)RegionEnum.UnitedKingdom, LowerBound = 0, UpperBound = 5000, TaxRate = 0 },
                new TaxBand() { RegionId = (int)RegionEnum.UnitedKingdom, LowerBound = 5000, UpperBound = 20000, TaxRate = 0.2M },
                new TaxBand() { RegionId = (int)RegionEnum.UnitedKingdom, LowerBound = 20000, UpperBound = null, TaxRate = 0.4M },
                new TaxBand() { RegionId = (int)RegionEnum.Ireland, LowerBound = 0, UpperBound = 5000, TaxRate = 0 },
                new TaxBand() { RegionId = (int)RegionEnum.Ireland, LowerBound = 5000, UpperBound = 20000, TaxRate = 0.2M },
                new TaxBand() { RegionId = (int)RegionEnum.Ireland, LowerBound = 20000, UpperBound = 40000, TaxRate = 0.4M },
                new TaxBand() { RegionId = (int)RegionEnum.Ireland, LowerBound = 40000, UpperBound = null, TaxRate = 0.6M },
                new TaxBand() { RegionId = (int)RegionEnum.France, LowerBound = 0, UpperBound = 5000, TaxRate = 0 },
                new TaxBand() { RegionId = (int)RegionEnum.France, LowerBound = 5000, UpperBound = 20000, TaxRate = 0.25M },
                new TaxBand() { RegionId = (int)RegionEnum.France, LowerBound = 20000, UpperBound = 40000, TaxRate = 0.55M },
                new TaxBand() { RegionId = (int)RegionEnum.France, LowerBound = 40000, UpperBound = null, TaxRate = 0.7M },                
            };

            context.TaxBands.AddRange(taxBands);
            context.SaveChanges();
        }
    }
}
