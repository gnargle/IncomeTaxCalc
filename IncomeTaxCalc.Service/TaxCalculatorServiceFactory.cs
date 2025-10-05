using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using IncomeTaxCalc.Services.TaxCalculators;
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
        private Dictionary<RegionDtoEnum, BaseRegionTaxCalculatorService> _calculatorServiceDictionary;
        public TaxCalculatorServiceFactory(IRegionService regionService)
        {
            _regionService = regionService;
            _calculatorServiceDictionary = new Dictionary<RegionDtoEnum, BaseRegionTaxCalculatorService>();
        }

        public BaseRegionTaxCalculatorService GetTaxCalculatorService(RegionDtoEnum region)
        {
            if (_calculatorServiceDictionary.TryGetValue(region, out var result))
                return result;
            switch (region)
            {
                case RegionDtoEnum.UnitedKingdom:
                    result = new UKTaxCalculatorService(_regionService); break;
                case RegionDtoEnum.France:
                    result = new FranceTaxCalculatorService(_regionService); break;
                case RegionDtoEnum.Ireland:
                    result = new IrelandTaxCalculatorService(_regionService); break;
                default:
                    throw new ArgumentException("Invalid region specified");
            }
            _calculatorServiceDictionary.Add(region, result);
            return result;
        }
    }
}
