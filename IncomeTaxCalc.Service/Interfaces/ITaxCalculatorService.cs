using IncomeTaxCalc.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.Interfaces
{
    public interface ITaxCalculatorService
    {
        public Task<TaxCalcResultDto> PerformTaxCalcAsync(TaxCalcRequestDto request, CancellationToken cancellationToken = default);
    }
}
