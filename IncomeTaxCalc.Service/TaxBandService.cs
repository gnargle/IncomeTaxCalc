using IncomeTaxCalc.Database;
using IncomeTaxCalc.Database.Models;
using IncomeTaxCalc.Database.Repositories.Interfaces;
using IncomeTaxCalc.DTOs;
using IncomeTaxCalc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services
{
    public class TaxBandService : ITaxBandService
    {
        private readonly ITaxBandRepository _taxBandRepository;
        public TaxBandService(ITaxBandRepository taxBandRepository) {
            _taxBandRepository = taxBandRepository;
        }
        public async Task<TaxBandDto> GetTaxBandAsync(int taxBandId, CancellationToken cancellationToken = default)
        {
            var taxBandModel = await _taxBandRepository.GetTaxBandAsync(taxBandId, cancellationToken);

            if (taxBandModel == null)
                throw new ArgumentException($"Tax Band {taxBandId} could not be found.");

            return MapModel(taxBandModel);
        }

        public async Task<IEnumerable<TaxBandDto>> GetTaxBandsForRegionsAsync(int regionId, CancellationToken cancellationToken = default)
        {
            var taxBandsModels = await _taxBandRepository.GetTaxBandsForRegionAsync(regionId, cancellationToken);

            return taxBandsModels.Select(m => MapModel(m));
        }

        private TaxBandDto MapModel(TaxBand taxBandModel)
        {
            return new TaxBandDto()
            {
                TaxBandId = taxBandModel.TaxBandId,
                RegionId = taxBandModel.RegionId,
                LowerBound = taxBandModel.LowerBound,
                UpperBound = taxBandModel.UpperBound,
                TaxRate = taxBandModel.TaxRate,
                Region = new RegionDto()
                {
                    RegionId = taxBandModel.Region.RegionId,
                    RegionName = taxBandModel.Region.RegionName,
                }
            };
        }
    }
}
