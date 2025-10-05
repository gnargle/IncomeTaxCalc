using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.TaxCalculators
{
    public class UKTaxCalculatorService : BaseTaxCalculatorService
    {
        public UKTaxCalculatorService(IRegionService regionService) : base(regionService, RegionDtoEnum.UnitedKingdom)
        {
        }
        public async override Task<TaxCalcResultDto> CalculateTaxAsync(TaxCalcRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await base.CalculateTaxAsync(request, cancellationToken);
            if (!string.IsNullOrWhiteSpace(result.Error))
            {
                return result;
            }
            decimal grossAnnual = request.GrossAnnual;
            decimal taxPayableTotal = 0;

            foreach (var band in Region.TaxBands)
            {
                if (grossAnnual < band.LowerBound)
                {
                    //do nothing if we're below the lower bound of this band.
                    continue;
                }

                decimal taxableInBand = 0;
                if (!band.UpperBound.HasValue || grossAnnual < band.UpperBound)
                {
                    taxableInBand = grossAnnual - band.LowerBound;                    
                } else
                {
                    //entire amount is in play
                    taxableInBand = band.UpperBound.Value - band.LowerBound;                   
                }
                var bandTaxToPay = taxableInBand * band.TaxRate;
                taxPayableTotal += bandTaxToPay;
            }

            return new TaxCalcResultDto()
            {
                GrossAnnual = grossAnnual,
                GrossMonthly = grossAnnual / 12M,
                NetAnnual = grossAnnual - taxPayableTotal,
                NetMonthly = (grossAnnual - taxPayableTotal) / 12M,
                AnnualTaxPaid = taxPayableTotal,
                MonthlyTaxPaid = taxPayableTotal / 12M
            };
        }
    }
}
