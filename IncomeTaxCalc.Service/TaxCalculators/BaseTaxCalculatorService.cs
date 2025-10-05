using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.TaxCalculators
{
    public abstract class BaseTaxCalculatorService
    {
        protected readonly IRegionService _regionService;
        protected readonly RegionDtoEnum _regionEnum;
        protected RegionDto? Region { get; private set; }
        public BaseTaxCalculatorService(IRegionService regionService, RegionDtoEnum regionEnum)
        {
            _regionService = regionService;
            _regionEnum = regionEnum;
        }
        public virtual async Task<TaxCalcResultDto> CalculateTaxAsync(TaxCalcRequestDto request, CancellationToken cancellationToken = default)
        {
            var error = CheckRequestDto(request);
            await FetchRegionAsync(cancellationToken);
            return new TaxCalcResultDto()
            {
                Error = error,
            };
        }

        private string CheckRequestDto(TaxCalcRequestDto request)
        {
            if (request == null)
            {
                return "Request is null";
            }

            if (request.GrossAnnual < 0)
            {
                return "Gross Annual Salary is less than 0.";
            }
            return string.Empty;
        }
        protected async Task FetchRegionAsync(CancellationToken cancellationToken = default)
        {
            if (Region == null)
                Region = await _regionService.GetRegionAsync(_regionEnum, cancellationToken);
        }
    }
}
