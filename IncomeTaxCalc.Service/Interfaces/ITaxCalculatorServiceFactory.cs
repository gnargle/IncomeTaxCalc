using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.TaxCalculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.Interfaces
{
    public interface ITaxCalculatorServiceFactory
    {
        public BaseTaxCalculatorService GetTaxCalculatorService(RegionDtoEnum region);
    }
}
