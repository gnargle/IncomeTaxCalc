using IncomeTaxCalc.Database.Models;
using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using IncomeTaxCalc.Services.TaxCalculators;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Shouldly;

namespace IncomeTaxCalc.Services.Tests
{
    public class UKTaxCalculatorServiceTests
    {
        private UKTaxCalculatorService _uKTaxCalculatorService;
        [SetUp]
        public void Setup()
        {
            var regionServiceMock = new Mock<IRegionService>();
            var region = new RegionDto()
            {
                RegionId = (int)RegionDtoEnum.UnitedKingdom,
                RegionName = "United Kingdom",
                TaxBands = new List<TaxBandDto>()
                {
                    new TaxBandDto() { RegionId = (int)RegionDtoEnum.UnitedKingdom, LowerBound = 0, UpperBound = 5000, TaxRate = 0 },
                    new TaxBandDto() { RegionId = (int)RegionDtoEnum.UnitedKingdom, LowerBound = 5000, UpperBound = 20000, TaxRate = 0.2M },
                    new TaxBandDto() { RegionId = (int)RegionDtoEnum.UnitedKingdom, LowerBound = 20000, UpperBound = null, TaxRate = 0.4M },
                }
            };
            regionServiceMock.Setup(m => m.GetRegionAsync(RegionDtoEnum.UnitedKingdom, default)).Returns(Task.FromResult(region));
            var memoryCacheMock = new NullMemoryCache();
            _uKTaxCalculatorService = new UKTaxCalculatorService(regionServiceMock.Object, memoryCacheMock);
        }

        [TestCase(40000, 29000)]
        [TestCase(19000, 16200)]
        [TestCase(5000, 5000)]
        [TestCase(5010, 5008)]
        public async Task GrossAnnualValid_OK(decimal grossAnnual, decimal netAnnual)
        {
            //Setup
            var request = new TaxCalcRequestDto()
            {
                RegionId = RegionDtoEnum.UnitedKingdom,
                GrossAnnual = grossAnnual
            };

            //Act
            var result = await _uKTaxCalculatorService.CalculateTaxAsync(request);

            //Assert
            result.ShouldNotBeNull();
            result.GrossAnnual.ShouldBe(grossAnnual);
            result.GrossMonthly.ShouldBe(Math.Round(grossAnnual / 12M,2));
            result.NetAnnual.ShouldBe(Math.Round(netAnnual,2));
            result.NetMonthly.ShouldBe(Math.Round(netAnnual / 12M,2));
            result.AnnualTaxPaid.ShouldBe(Math.Round(grossAnnual - netAnnual,2));
            result.MonthlyTaxPaid.ShouldBe(Math.Round((grossAnnual - netAnnual) / 12M,2));
        }

        [Test]
        public async Task GrossAnnualLessThanZero_Fail()
        {
            //Setup
            var request = new TaxCalcRequestDto()
            {
                RegionId = RegionDtoEnum.UnitedKingdom,
                GrossAnnual = -1
            };

            //Act
            var result = await _uKTaxCalculatorService.CalculateTaxAsync(request);


            //Assert
            result.ShouldNotBeNull();
            result.GrossAnnual.ShouldBeNull();
            result.GrossMonthly.ShouldBeNull();
            result.NetAnnual.ShouldBeNull();
            result.NetMonthly.ShouldBeNull();
            result.AnnualTaxPaid.ShouldBeNull();
            result.MonthlyTaxPaid.ShouldBeNull();
            result.Error.ShouldBe("Gross Annual Salary is less than 0.");
        }

        [Test]
        public async Task RequestNull_Fail()
        {
            //Act
            var result = await _uKTaxCalculatorService.CalculateTaxAsync(null);


            //Assert
            result.ShouldNotBeNull();
            result.GrossAnnual.ShouldBeNull();
            result.GrossMonthly.ShouldBeNull();
            result.NetAnnual.ShouldBeNull();
            result.NetMonthly.ShouldBeNull();
            result.AnnualTaxPaid.ShouldBeNull();
            result.MonthlyTaxPaid.ShouldBeNull();
            result.Error.ShouldBe("Request is null");
        }
    }
}