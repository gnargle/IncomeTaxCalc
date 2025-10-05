using Azure.Core;
using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.TaxCalculators
{
    public abstract class BaseRegionTaxCalculatorService
    {
        protected readonly IRegionService _regionService;
        protected readonly RegionDtoEnum _regionEnum;
        protected readonly IMemoryCache _memoryCache;
        protected RegionDto? Region { get; private set; }
        public BaseRegionTaxCalculatorService(IRegionService regionService, RegionDtoEnum regionEnum, IMemoryCache memoryCache)
        {
            _regionService = regionService;
            _regionEnum = regionEnum;
            _memoryCache = memoryCache;
        }
        public virtual async Task<TaxCalcResultDto> CalculateTaxAsync(TaxCalcRequestDto request, CancellationToken cancellationToken = default)
        {            
            var error = CheckRequestDto(request);
            await FetchRegionAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(error))
            {
                if (_memoryCache.TryGetValue($"result:{request.GrossAnnual}", out TaxCalcResultDto completeResult))
                {
                    return completeResult;
                }
            }
            
            return new TaxCalcResultDto()
            {
                Error = error,
            };
        }

        protected void StoreResultInCache(TaxCalcResultDto result)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _memoryCache.Set($"result:{result.GrossAnnual}", result, cacheEntryOptions);
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
