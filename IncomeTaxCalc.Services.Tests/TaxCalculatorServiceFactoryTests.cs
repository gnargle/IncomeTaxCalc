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
    public class TaxCalculatorServiceFactoryTests
    {
        private TaxCalculatorServiceFactory _taxCalculatorServiceFactory;
        [SetUp]
        public void Setup()
        {
            var regionServiceMock = new Mock<IRegionService>();
            _taxCalculatorServiceFactory = new TaxCalculatorServiceFactory(regionServiceMock.Object);
        }

        [Test]
        public async Task GetUKCalculator_OK()
        {
            //Act
            var result = _taxCalculatorServiceFactory.GetTaxCalculatorService(RegionDtoEnum.UnitedKingdom);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(UKTaxCalculatorService));
        }

        [Test]
        public async Task GetFranceCalculator_OK()
        {
            //Act
            var result = _taxCalculatorServiceFactory.GetTaxCalculatorService(RegionDtoEnum.France);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(FranceTaxCalculatorService));
        }

        [Test]
        public async Task GetIrelandCalculator_OK()
        {
            //Act
            var result = _taxCalculatorServiceFactory.GetTaxCalculatorService(RegionDtoEnum.Ireland);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(IrelandTaxCalculatorService));
        }

        [Test]
        public async Task EnumNone_Fail()
        {
            //Act & Assert
            Should.Throw<ArgumentException>(() => _taxCalculatorServiceFactory.GetTaxCalculatorService(RegionDtoEnum.None))
                .Message.ShouldBe("Invalid region specified");

        }
    }
}
