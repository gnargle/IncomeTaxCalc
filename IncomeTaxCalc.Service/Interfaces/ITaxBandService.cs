using IncomeTaxCalc.Database.Models;
using IncomeTaxCalc.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.Interfaces
{
    public interface ITaxBandService
    {
        public Task<TaxBandDto> GetTaxBandAsync(int taxBandId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<TaxBandDto>> GetTaxBandsForRegionsAsync(int regionId, CancellationToken cancellationToken = default);
    }
}
