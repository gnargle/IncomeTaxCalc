using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly ITaxCalculatorServiceFactory _taxCalculatorServiceFactory;
        public TaxCalculatorService(ITaxCalculatorServiceFactory taxCalculatorServiceFactory)
        {
            _taxCalculatorServiceFactory = taxCalculatorServiceFactory;
        }
        public async Task<TaxCalcResultDto> PerformTaxCalcAsync(TaxCalcRequestDto request, CancellationToken cancellationToken = default)
        {
            var calculatorService = _taxCalculatorServiceFactory.GetTaxCalculatorService(request.RegionId);
            var result = await calculatorService.CalculateTaxAsync(request, cancellationToken);
            return result;
        }
    }
}
