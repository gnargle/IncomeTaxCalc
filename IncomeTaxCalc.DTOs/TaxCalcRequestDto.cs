using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.DTOs
{
    public class TaxCalcRequestDto
    {
        public RegionDtoEnum RegionId { get; set; }
        public decimal GrossAnnual { get; set; }
    }
}
