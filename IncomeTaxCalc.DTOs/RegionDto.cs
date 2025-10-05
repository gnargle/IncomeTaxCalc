namespace IncomeTaxCalc.DTOs
{
    public class RegionDto
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; } = string.Empty;
        public List<TaxBandDto> TaxBands { get; set; } = new List<TaxBandDto>();
    }
}
