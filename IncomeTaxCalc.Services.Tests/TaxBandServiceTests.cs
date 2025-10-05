using IncomeTaxCalc.Database.Models;
using IncomeTaxCalc.Database.Repositories.Interfaces;
using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.Tests
{
    public class TaxBandServiceTests
    {
        [Test]
        public async Task GetTaxBandsByID_OK()
        {
            var taxBandRepoMock = new Mock<ITaxBandRepository>();
            int taxBandId = 1;
            int regionId = (int)RegionEnum.UnitedKingdom;
            var taxBand = new TaxBand()
            {
                TaxBandId = taxBandId,
                TaxRate = 0.2M,
                LowerBound = 0,
                UpperBound = 10000,
                RegionId = regionId,
                Region = new Region()
                {
                    RegionId = regionId,
                    RegionName = "United Kingdom"
                }
            };
            taxBandRepoMock.Setup(m => m.GetTaxBandAsync(taxBandId, default)).Returns(Task.FromResult(taxBand));
            var _taxBandService = new TaxBandService(taxBandRepoMock.Object);

            //Act
            var result = await _taxBandService.GetTaxBandAsync(taxBandId, default);

            //Assert
            result.ShouldNotBeNull();
            result.TaxBandId.ShouldBe(taxBandId);
            result.TaxRate.ShouldBe(taxBand.TaxRate);
            result.LowerBound.ShouldBe(taxBand.LowerBound);
            result.UpperBound.ShouldBe(taxBand.UpperBound);
            result.RegionId.ShouldBe(regionId);
            result.Region.RegionId.ShouldBe(regionId);
            result.Region.RegionName.ShouldBe(taxBand.Region.RegionName);
        }

        [Test]
        public async Task GetTaxBandByID_NotFound()
        {
            var taxBandRepoMock = new Mock<ITaxBandRepository>();
            int taxBandId = 1;
            taxBandRepoMock.Setup(m => m.GetTaxBandAsync(taxBandId, default)).Returns(Task.FromResult<TaxBand?>(null));
            var _taxBandService = new TaxBandService(taxBandRepoMock.Object);

            //Act & Assert
            Should.Throw<ArgumentException>(() => _taxBandService.GetTaxBandAsync(taxBandId))
                .Message.ShouldBe($"Tax Band {taxBandId} could not be found.");
        }

        [TestCase(RegionEnum.UnitedKingdom)]
        [TestCase(RegionEnum.France)]
        [TestCase(RegionEnum.Ireland)]
        public async Task GetTaxBandsForRegion_OK(RegionEnum regionEnum)
        {
            var taxBandRepoMock = new Mock<ITaxBandRepository>();
            int regionId = (int)regionEnum;

            var ukRegion = new Region()
            {
                RegionId = (int)RegionEnum.UnitedKingdom,
                RegionName = "United Kingdom",
            };
            IEnumerable<TaxBand> taxBandsUK = new List<TaxBand>()
            {
                new TaxBand() { RegionId = (int)RegionEnum.UnitedKingdom, LowerBound = 0, UpperBound = 5000, TaxRate = 0, Region = ukRegion },
                new TaxBand() { RegionId = (int)RegionEnum.UnitedKingdom, LowerBound = 5000, UpperBound = 20000, TaxRate = 0.2M, Region = ukRegion },
                new TaxBand() { RegionId = (int)RegionEnum.UnitedKingdom, LowerBound = 20000, UpperBound = null, TaxRate = 0.4M, Region = ukRegion },
            };

            var franceRegion = new Region()
            {
                RegionId = (int)RegionEnum.France,
                RegionName = "France"
            };
            IEnumerable<TaxBand> taxBandsFrance = new List<TaxBand>()
            {
                new TaxBand() { RegionId = (int)RegionEnum.France, LowerBound = 0, UpperBound = 5000, TaxRate = 0, Region = franceRegion },
                new TaxBand() { RegionId = (int)RegionEnum.France, LowerBound = 5000, UpperBound = 20000, TaxRate = 0.25M, Region = franceRegion },
                new TaxBand() { RegionId = (int)RegionEnum.France, LowerBound = 20000, UpperBound = 40000, TaxRate = 0.55M, Region = franceRegion },
                new TaxBand() { RegionId = (int)RegionEnum.France, LowerBound = 40000, UpperBound = null, TaxRate = 0.7M, Region = franceRegion },
            };

            var irelandRegion = new Region()
            {
                RegionId = (int)RegionEnum.Ireland,
                RegionName = "Ireland"
            };
            IEnumerable<TaxBand> taxBandsIreland = new List<TaxBand>()
            {
                new TaxBand() { RegionId = (int)RegionEnum.Ireland, LowerBound = 0, UpperBound = 5000, TaxRate = 0, Region = irelandRegion },
                new TaxBand() { RegionId = (int)RegionEnum.Ireland, LowerBound = 5000, UpperBound = 20000, TaxRate = 0.2M, Region = irelandRegion },
                new TaxBand() { RegionId = (int)RegionEnum.Ireland, LowerBound = 20000, UpperBound = 40000, TaxRate = 0.4M, Region = irelandRegion },
                new TaxBand() { RegionId = (int)RegionEnum.Ireland, LowerBound = 40000, UpperBound = null, TaxRate = 0.6M, Region = irelandRegion },
            };

            taxBandRepoMock.Setup(m => m.GetTaxBandsForRegionAsync((int)RegionEnum.UnitedKingdom, default)).Returns(Task.FromResult(taxBandsUK));
            taxBandRepoMock.Setup(m => m.GetTaxBandsForRegionAsync((int)RegionEnum.France, default)).Returns(Task.FromResult(taxBandsFrance));
            taxBandRepoMock.Setup(m => m.GetTaxBandsForRegionAsync((int)RegionEnum.Ireland, default)).Returns(Task.FromResult(taxBandsIreland));

            var _taxBandService = new TaxBandService(taxBandRepoMock.Object);

            //Act
            var result = await _taxBandService.GetTaxBandsForRegionsAsync((int)regionEnum, default);

            //Assert
            result.ShouldNotBeNull();
            IEnumerable<TaxBand> compareData = new List<TaxBand>();
            switch (regionEnum)
            {
                case RegionEnum.UnitedKingdom:
                    compareData = taxBandsUK;
                    break;
                case RegionEnum.Ireland:
                    compareData = taxBandsIreland;
                    break;
                case RegionEnum.France:
                    compareData = taxBandsFrance;
                    break;                        
            }
            Should.Equals(result.Count(), compareData.Count());
            var resultFirst = result.First();
            var compareFirst = compareData.First();
            resultFirst.TaxBandId.ShouldBe(compareFirst.TaxBandId);
            resultFirst.TaxRate.ShouldBe(compareFirst.TaxRate);
            resultFirst.LowerBound.ShouldBe(compareFirst.LowerBound);
            resultFirst.UpperBound.ShouldBe(compareFirst.UpperBound);
            resultFirst.RegionId.ShouldBe(compareFirst.RegionId);
            resultFirst.Region.RegionId.ShouldBe(compareFirst.Region.RegionId);
            resultFirst.Region.RegionName.ShouldBe(compareFirst.Region.RegionName);
        }
    }
}
