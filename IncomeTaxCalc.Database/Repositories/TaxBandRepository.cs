using IncomeTaxCalc.Database.Models;
using IncomeTaxCalc.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Database.Repositories
{
    public sealed class TaxBandRepository : ITaxBandRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceScopeFactory _scopeFactory;
        public TaxBandRepository(IServiceScopeFactory scopeFactory, IMemoryCache memoryCache)
        {
            _scopeFactory = scopeFactory;
            _memoryCache = memoryCache;
        }
        public async Task<TaxBand> GetTaxBandAsync(int taxBandId, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"taxBand:{taxBandId}";
            if (!_memoryCache.TryGetValue(cacheKey, out TaxBand taxBand))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
                    var dbTaxBand = await context.TaxBands.FirstOrDefaultAsync(b => b.TaxBandId == taxBandId);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    _memoryCache.Set(cacheKey, dbTaxBand, cacheEntryOptions);
                    return dbTaxBand;
                }
            }
            else
            {
                return taxBand;
            }
        }

        public async Task<IEnumerable<TaxBand>> GetTaxBandsForRegionAsync(int regionId, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"regionTaxBands:{regionId}";
            if (!_memoryCache.TryGetValue(cacheKey, out List<TaxBand> taxBands))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
                    var taxBandsDb = await context.TaxBands.Where(b => b.RegionId == regionId).ToListAsync(cancellationToken);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    _memoryCache.Set(cacheKey, taxBandsDb, cacheEntryOptions);
                    return taxBandsDb;
                }
            }
            else
            {
                return taxBands;
            }
        }
    }
}
