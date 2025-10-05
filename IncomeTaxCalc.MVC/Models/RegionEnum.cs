using System.ComponentModel.DataAnnotations;

namespace IncomeTaxCalc.MVC.Models
{
    public enum RegionEnum
    {
        [Display(Name ="United Kingdom")]
        UnitedKingdom = 1,
        [Display(Name = "France")]
        Ireland = 2,
        [Display(Name = "Ireland")]
        France = 3
    }
}
