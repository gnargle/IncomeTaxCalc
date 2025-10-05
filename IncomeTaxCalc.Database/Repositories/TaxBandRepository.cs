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
        private readonly TaxCalcContext _context;
        public TaxBandRepository(IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                _context = scope.ServiceProvider.GetRequiredService<TaxCalcContext>();
            }
        }
        public async Task<TaxBand> GetTaxBandAsync(int taxBandId, CancellationToken cancellationToken = default)
        {
            return await _context.TaxBands.FirstOrDefaultAsync(b => b.TaxBandId == taxBandId);
        }

        public async Task<IEnumerable<TaxBand>> GetTaxBandsForRegionAsync(int regionId, CancellationToken cancellationToken = default)
        {
            return await _context.TaxBands.Where(b => b.RegionId == regionId).ToListAsync(cancellationToken);
        }
    }
}
