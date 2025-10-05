using IncomeTaxCalc.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncomeTaxCalc.Database.Repositories.Interfaces
{
    public interface IRegionRepository
    {
        public Task<Region> GetRegionAsync(int regionId, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Region>> GetRegionsAsync(CancellationToken cancellationToken = default);
    }
}
