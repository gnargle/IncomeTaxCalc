using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.TaxCalculators
{
    public class IrelandTaxCalculatorService : BaseRegionTaxCalculatorService
    {
        public IrelandTaxCalculatorService(IRegionService regionService) : base(regionService, RegionDtoEnum.Ireland)
        {
        }

        public override async Task<TaxCalcResultDto> CalculateTaxAsync(TaxCalcRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await base.CalculateTaxAsync(request, cancellationToken);
            //actual tax calculations are I'm sure different to the UK calculator but I don't think understanding foreign tax laws are
            //a part of this exercise, so this behaviour has been copied from the UK calc for simplicity/to demonstrate the multiple further tax
            //bands this solution can handle.
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
                }
                else
                {
                    //entire amount is in play
                    taxableInBand = band.UpperBound.Value - band.LowerBound;
                }
                var bandTaxToPay = taxableInBand * band.TaxRate;
                taxPayableTotal += bandTaxToPay;
            }

            return new TaxCalcResultDto()
            {
                GrossAnnual = Math.Round(grossAnnual, 2),
                GrossMonthly = Math.Round(grossAnnual / 12M, 2),
                NetAnnual = Math.Round(grossAnnual - taxPayableTotal, 2),
                NetMonthly = Math.Round((grossAnnual - taxPayableTotal) / 12M, 2),
                AnnualTaxPaid = Math.Round(taxPayableTotal, 2),
                MonthlyTaxPaid = Math.Round(taxPayableTotal / 12M, 2)
            };
        }
    }
}
