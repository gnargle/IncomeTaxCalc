using IncomeTaxCalc.Database;
using IncomeTaxCalc.Database.Models;
using IncomeTaxCalc.Database.Repositories.Interfaces;
using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace IncomeTaxCalc.Services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        public RegionService(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<RegionDto> GetRegionAsync(RegionDtoEnum regionId, CancellationToken cancellationToken = default)
        {
            var regionModel = await _regionRepository.GetRegionAsync((int)regionId, cancellationToken);

            if (regionModel == null)
                throw new ArgumentException($"Region {regionId} not found");

            return MapModel(regionModel);
        }

        public async Task<IEnumerable<RegionDto>> GetRegionsAsync(CancellationToken cancellationToken = default)
        {
            var regionModels = await _regionRepository.GetRegionsAsync(cancellationToken);
            return regionModels.Select(m => MapModel(m));
        }

        private RegionDto MapModel(Region regionModel)
        {
            return new RegionDto()
            {
                RegionId = regionModel.RegionId,
                RegionName = regionModel.RegionName,
                TaxBands = regionModel.TaxBands.Select(t => new TaxBandDto()
                {
                    TaxBandId = t.TaxBandId,
                    RegionId = t.RegionId,
                    LowerBound = t.LowerBound,
                    UpperBound = t.UpperBound,
                    TaxRate = t.TaxRate,
                }).ToList()
            };
        }
    }
}
