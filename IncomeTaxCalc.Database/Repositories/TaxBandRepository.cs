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
    public sealed class TaxBandRepository : ITaxBandRepository
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public TaxBandRepository(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task<TaxBand> GetTaxBandAsync(int taxBandId, CancellationToken cancellationToken = default)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
                return await context.TaxBands.FirstOrDefaultAsync(b => b.TaxBandId == taxBandId);
            }
        }

        public async Task<IEnumerable<TaxBand>> GetTaxBandsForRegionAsync(int regionId, CancellationToken cancellationToken = default)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
                return await context.TaxBands.Where(b => b.RegionId == regionId).ToListAsync(cancellationToken);
            }
        }
    }
}
