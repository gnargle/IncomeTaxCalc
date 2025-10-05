using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.MVC.Models;
using IncomeTaxCalc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IncomeTaxCalc.MVC.Controllers
{
    public class TaxCalcController : Controller
    {
        private readonly ILogger<TaxCalcController> _logger;
        private readonly ITaxCalculatorService _taxCalcService;

        public TaxCalcController(ILogger<TaxCalcController> logger, ITaxCalculatorService taxCalcService)
        {
            _logger = logger;
            _taxCalcService = taxCalcService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new TaxCalcFormModel()
            {
                GrossAnnual = 0,
                RegionId = RegionEnum.UnitedKingdom
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCalc(TaxCalcFormModel model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            var request = new TaxCalcRequestDto()
            {
                GrossAnnual = model.GrossAnnual,
                RegionId = (RegionDtoEnum)model.RegionId,
            };
            var resultDto = await _taxCalcService.PerformTaxCalcAsync(request, cancellationToken);

            if (resultDto == null)
                return Error();
            
            model.GrossMonthly = resultDto.GrossMonthly;
            model.NetAnnual = resultDto.NetAnnual;
            model.NetMonthly = resultDto.NetMonthly;
            model.AnnualTaxPaid = resultDto.AnnualTaxPaid;
            model.MonthlyTaxPaid = resultDto.MonthlyTaxPaid;
            model.Error = resultDto.Error;

            return View("CalculationResults", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
