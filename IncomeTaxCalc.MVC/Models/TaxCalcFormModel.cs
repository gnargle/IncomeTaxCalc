using System.ComponentModel.DataAnnotations;

namespace IncomeTaxCalc.MVC.Models
{
    public class TaxCalcFormModel
    {
        [Display(Name ="Region")]
        [Required(ErrorMessage = "{0} is required")]
        public RegionEnum RegionId {  get; set; }
        [Display(Name = "Gross Annual Income")]
        [Required(ErrorMessage = "Gross Annual Income must be a decimal value.")]
        public decimal GrossAnnual { get; set; }
        [Display(Name = "Gross Monthly Income")]
        public decimal? GrossMonthly { get; set; }
        [Display(Name = "Net Annual Income")]
        public decimal? NetAnnual { get; set; }
        [Display(Name = "Net Monthly Income")]
        public decimal? NetMonthly { get; set; }
        [Display(Name = "Annual Tax Paid")]
        public decimal? AnnualTaxPaid { get; set; }
        [Display(Name = "Monthly Tax Paid")]
        public decimal? MonthlyTaxPaid { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
