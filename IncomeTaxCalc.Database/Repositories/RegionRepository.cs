using IncomeTaxCalc.Database.Models;
using IncomeTaxCalc.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        private readonly IServiceScopeFactory _scopeFactory;
        public RegionRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<Region> GetRegionAsync(int regionId, CancellationToken cancellationToken = default)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
                return await context.Regions.Include(r => r.TaxBands).FirstOrDefaultAsync(r => r.RegionId == regionId, cancellationToken);
            }
        }

        public async Task<IEnumerable<Region>> GetRegionsAsync(CancellationToken cancellationToken = default)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
                return await context.Regions.ToListAsync(cancellationToken);
            }
        }
    }
}
