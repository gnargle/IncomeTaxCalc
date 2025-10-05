using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.DTOs
{
    public class TaxCalcResultDto
    {
        public decimal? GrossAnnual { get; set; }
        public decimal? GrossMonthly { get; set; }
        public decimal? NetAnnual { get; set; }
        public decimal? NetMonthly { get; set; }
        public decimal? AnnualTaxPaid { get; set; }
        public decimal? MonthlyTaxPaid { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
