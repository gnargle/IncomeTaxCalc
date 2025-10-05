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
    public sealed class RegionRepository : IRegionRepository
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceScopeFactory _scopeFactory;
        public RegionRepository(IServiceScopeFactory scopeFactory, IMemoryCache memoryCache)
        {
            _scopeFactory = scopeFactory;
            _memoryCache = memoryCache;
        }

        public async Task<Region> GetRegionAsync(int regionId, CancellationToken cancellationToken = default)
        {
            string cacheKey = $"region:{regionId}";
            if (!_memoryCache.TryGetValue(cacheKey, out Region region))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
                    var dbRegion = await context.Regions.Include(r => r.TaxBands).FirstOrDefaultAsync(r => r.RegionId == regionId, cancellationToken);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    _memoryCache.Set(cacheKey, dbRegion, cacheEntryOptions);
                    return dbRegion;
                }
            }
            else
            {
                return region;
            }
        }

        public async Task<IEnumerable<Region>> GetRegionsAsync(CancellationToken cancellationToken = default)
        {
            string cacheKey = "allRegions";
            if (!_memoryCache.TryGetValue(cacheKey, out List<Region> regions))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
                    var dbRegions = await context.Regions.ToListAsync(cancellationToken);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    _memoryCache.Set(cacheKey, dbRegions, cacheEntryOptions);
                    return dbRegions;
                }
            } else
            {
                return regions;
            }
        }
    }
}
