using IncomeTaxCalc.Database.Models;
using IncomeTaxCalc.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Services.Interfaces
{
    public interface IRegionService
    {
        public Task<RegionDto> GetRegionAsync(RegionDtoEnum regionId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<RegionDto>> GetRegionsAsync(CancellationToken cancellationToken = default);
    }
}
