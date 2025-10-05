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
        private Dictionary<RegionDtoEnum, BaseRegionTaxCalculatorService> _calculatorServiceDictionary;
        public TaxCalculatorServiceFactory(IRegionService regionService, IMemoryCache memoryCache)
        {
            _regionService = regionService;
            _memoryCache = memoryCache;
            _calculatorServiceDictionary = new Dictionary<RegionDtoEnum, BaseRegionTaxCalculatorService>();
        }

        public BaseRegionTaxCalculatorService GetTaxCalculatorService(RegionDtoEnum region)
        {
            if (_calculatorServiceDictionary.TryGetValue(region, out var result))
                return result;
            switch (region)
            {
                case RegionDtoEnum.UnitedKingdom:
                    result = new UKTaxCalculatorService(_regionService, _memoryCache); break;
                case RegionDtoEnum.France:
                    result = new FranceTaxCalculatorService(_regionService, _memoryCache); break;
                case RegionDtoEnum.Ireland:
                    result = new IrelandTaxCalculatorService(_regionService, _memoryCache); break;
                default:
                    throw new ArgumentException("Invalid region specified");
            }
            _calculatorServiceDictionary.Add(region, result);
            return result;
        }
    }
}
