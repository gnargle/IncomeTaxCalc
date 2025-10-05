using IncomeTaxCalc.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Database.Repositories.Interfaces
{
    public interface ITaxBandRepository
    {
        public Task<TaxBand> GetTaxBandAsync(int taxBandId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<TaxBand>> GetTaxBandsForRegionAsync(int regionId, CancellationToken cancellationToken = default);
    }
}
