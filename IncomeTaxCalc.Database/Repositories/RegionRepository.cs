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
        private readonly TaxCalcContext _context;
        public RegionRepository(IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
            }
        }

        public async Task<Region> GetRegionAsync(int regionId, CancellationToken cancellationToken = default)
        {
            return await _context.Regions.FirstOrDefaultAsync(r => r.RegionId == regionId, cancellationToken);
        }

        public async Task<IEnumerable<Region>> GetRegionsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Regions.ToListAsync(cancellationToken);
        }
    }
}
