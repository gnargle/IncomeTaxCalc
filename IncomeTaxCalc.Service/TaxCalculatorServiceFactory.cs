using IncomeTaxCalc.Database.Models;
using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using IncomeTaxCalc.Services.TaxCalculators;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services
{
    public class TaxCalculatorServiceFactory : ITaxCalculatorServiceFactory
    {
        private readonly IRegionService _regionService;
        private readonly IMemoryCache _memoryCache;
        public TaxCalculatorServiceFactory(IRegionService regionService, IMemoryCache memoryCache)
        {
            _regionService = regionService;
            _memoryCache = memoryCache;
            _calculatorServiceDictionary = new Dictionary<RegionDtoEnum, BaseRegionTaxCalculatorService>();
        }

        public BaseRegionTaxCalculatorService GetTaxCalculatorService(RegionDtoEnum region)
        {
            string cacheKey = $"calcService:{region}";
            if (!_memoryCache.TryGetValue(cacheKey, out BaseRegionTaxCalculatorService service))
            {
                BaseRegionTaxCalculatorService newService;
                switch (region)
                {
                    case RegionDtoEnum.UnitedKingdom:
                        newService = new UKTaxCalculatorService(_regionService, _memoryCache); break;
                    case RegionDtoEnum.France:
                        newService = new FranceTaxCalculatorService(_regionService, _memoryCache); break;
                    case RegionDtoEnum.Ireland:
                        newService = new IrelandTaxCalculatorService(_regionService, _memoryCache); break;
                    default:
                        throw new ArgumentException("Invalid region specified");
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                _memoryCache.Set(cacheKey, newService, cacheEntryOptions);

                return newService;
            }
            else
            {
                return service;
            }
        }
    }
}
