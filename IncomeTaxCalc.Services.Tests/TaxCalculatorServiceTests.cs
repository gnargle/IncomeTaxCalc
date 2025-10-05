using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using IncomeTaxCalc.Services.TaxCalculators;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.Tests
{
    public class TaxCalculatorServiceTests
    {
        [Test]
        public async Task CalculateUKTax_OK()
        {
            //Setup
            var mockFactory = new Mock<ITaxCalculatorServiceFactory>();
            var mockRegionService = new Mock<IRegionService>();
            var mockUKService = new Mock<UKTaxCalculatorService>(mockRegionService.Object);
            mockFactory.Setup(x => x.GetTaxCalculatorService(DTOs.RegionDtoEnum.UnitedKingdom)).Returns(mockUKService.Object);

            var _taxCalculatorService = new TaxCalculatorService(mockFactory.Object);

            var request = new TaxCalcRequestDto()
            {
                GrossAnnual = 40000,
                RegionId = RegionDtoEnum.UnitedKingdom
            };
            var grossAnnual = 40000;
            var netAnnual = 29000;
            var expectedResult = new TaxCalcResultDto()
            {
                GrossAnnual = grossAnnual,
                NetAnnual = netAnnual,
                GrossMonthly = Math.Round(grossAnnual / 12M, 2),
                NetMonthly = Math.Round(netAnnual / 12M, 2),
                AnnualTaxPaid = grossAnnual - netAnnual,
                MonthlyTaxPaid = Math.Round((grossAnnual - netAnnual) / 12M, 2)
            };

            mockUKService.Setup(s => s.CalculateTaxAsync(request, default)).Returns(Task.FromResult(expectedResult));

            //Act
            var result = await _taxCalculatorService.PerformTaxCalcAsync(request);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public async Task CalculateFranceTax_OK()
        {
            //Setup
            var mockFactory = new Mock<ITaxCalculatorServiceFactory>();
            var mockRegionService = new Mock<IRegionService>();
            var mockFranceService = new Mock<FranceTaxCalculatorService>(mockRegionService.Object);
            mockFactory.Setup(x => x.GetTaxCalculatorService(DTOs.RegionDtoEnum.France)).Returns(mockFranceService.Object);

            var _taxCalculatorService = new TaxCalculatorService(mockFactory.Object);

            var request = new TaxCalcRequestDto()
            {
                GrossAnnual = 40000,
                RegionId = RegionDtoEnum.France
            };
            var grossAnnual = 40000;
            var netAnnual = 29000;
            var expectedResult = new TaxCalcResultDto()
            {
                GrossAnnual = grossAnnual,
                NetAnnual = netAnnual,
                GrossMonthly = Math.Round(grossAnnual / 12M, 2),
                NetMonthly = Math.Round(netAnnual / 12M, 2),
                AnnualTaxPaid = grossAnnual - netAnnual,
                MonthlyTaxPaid = Math.Round((grossAnnual - netAnnual) / 12M, 2)
            };

            mockFranceService.Setup(s => s.CalculateTaxAsync(request, default)).Returns(Task.FromResult(expectedResult));

            //Act
            var result = await _taxCalculatorService.PerformTaxCalcAsync(request);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBe(expectedResult);
        }

        [Test]
        public async Task CalculateIrelandTax_OK()
        {
            //Setup
            var mockFactory = new Mock<ITaxCalculatorServiceFactory>();
            var mockRegionService = new Mock<IRegionService>();
            var mockIrelandService = new Mock<IrelandTaxCalculatorService>(mockRegionService.Object);
            mockFactory.Setup(x => x.GetTaxCalculatorService(DTOs.RegionDtoEnum.Ireland)).Returns(mockIrelandService.Object);

            var _taxCalculatorService = new TaxCalculatorService(mockFactory.Object);

            var request = new TaxCalcRequestDto()
            {
                GrossAnnual = 40000,
                RegionId = RegionDtoEnum.Ireland
            };
            var grossAnnual = 40000;
            var netAnnual = 29000;
            var expectedResult = new TaxCalcResultDto()
            {
                GrossAnnual = grossAnnual,
                NetAnnual = netAnnual,
                GrossMonthly = Math.Round(grossAnnual / 12M, 2),
                NetMonthly = Math.Round(netAnnual / 12M, 2),
                AnnualTaxPaid = grossAnnual - netAnnual,
                MonthlyTaxPaid = Math.Round((grossAnnual - netAnnual) / 12M, 2)
            };

            mockIrelandService.Setup(s => s.CalculateTaxAsync(request, default)).Returns(Task.FromResult(expectedResult));

            //Act
            var result = await _taxCalculatorService.PerformTaxCalcAsync(request);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBe(expectedResult);
        }
    }
}
