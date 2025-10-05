using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.DTOs
{
    public class TaxBandDto
    {
        public int TaxBandId { get; set; }
        public int RegionId { get; set; }
        public decimal LowerBound { get; set; }
        public decimal? UpperBound { get; set; }
        public decimal TaxRate { get; set; }
        public RegionDto Region { get; set; } = new RegionDto();
    }
}
