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
    public class RegionServiceTests
    {

        [TestCase(RegionDtoEnum.UnitedKingdom)]
        [TestCase(RegionDtoEnum.France)]
        [TestCase(RegionDtoEnum.Ireland)]
        public async Task GetRegionById_OK(RegionDtoEnum regionDtoEnum)
        {
            //Setup
            var regionRepoMock = new Mock<IRegionRepository>();
            var regionMock = new Region()
            {
                RegionId = (int)regionDtoEnum,
                RegionName = regionDtoEnum.ToString(),
                TaxBands = new List<TaxBand>()
            };
            regionRepoMock.Setup(m => m.GetRegionAsync((int)regionDtoEnum, default)).Returns(Task.FromResult(regionMock));
            var _regionService = new RegionService(regionRepoMock.Object);

            //Act
            var result = await _regionService.GetRegionAsync(regionDtoEnum);

            //Assert
            result.ShouldNotBeNull();
            Should.Equals(result.RegionId, (int)regionDtoEnum);
            Should.Equals(result.RegionName, regionDtoEnum.ToString());
        }

        [Test]
        public async Task GetRegionById_NotFound()
        {
            //Setup
            var regionRepoMock = new Mock<IRegionRepository>();
            regionRepoMock.Setup(m => m.GetRegionAsync((int)RegionDtoEnum.None, default)).Returns(Task.FromResult<Region?>(null));
            var _regionService = new RegionService(regionRepoMock.Object);

            //Act & Assert
            Should.Throw<ArgumentException>(() => _regionService.GetRegionAsync(RegionDtoEnum.None, default))
                .Message.ShouldBe($"Region {RegionDtoEnum.None} not found");
        }

        [Test]
        public async Task GetRegions_OK()
        {
            //Setup
            var regionRepoMock = new Mock<IRegionRepository>();
            IEnumerable<Region> regionMocks = new List<Region>()
            {
                new Region()
                {
                    RegionId = (int)RegionDtoEnum.UnitedKingdom,
                    RegionName = "United Kingdom",
                    TaxBands = new List<TaxBand>(),
                },
                new Region()
                {
                    RegionId = (int)RegionDtoEnum.France,
                    RegionName = "France",
                    TaxBands = new List<TaxBand>(),
                },
                new Region()
                {
                    RegionId = (int)RegionDtoEnum.Ireland,
                    RegionName = "Ireland",
                    TaxBands = new List<TaxBand>(),
                },
            };
            regionRepoMock.Setup(m => m.GetRegionsAsync(default)).Returns(Task.FromResult(regionMocks));
            var _regionService = new RegionService(regionRepoMock.Object);

            //Act
            var result = await _regionService.GetRegionsAsync(default);

            //Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(regionMocks.Count());
            var ukModel = regionMocks.FirstOrDefault(m => m.RegionId == (int)RegionDtoEnum.UnitedKingdom);
            var ukDto = result.FirstOrDefault(m => m.RegionId == (int)RegionDtoEnum.UnitedKingdom);
            ukDto.ShouldNotBeNull();
            Should.Equals(ukModel!.RegionId, ukDto.RegionId);
            Should.Equals(ukModel.RegionName, ukDto.RegionName);
            Should.Equals(ukModel.TaxBands.Count, ukDto.TaxBands.Count);
        }
    }
}
