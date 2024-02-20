using Core.DTO;

namespace Database.Entities.Utils
{
    public static class ScanTargetMapper
    {
        public static ScanTargetDto ToDto(this ScanTarget scanTarget)
        {
            return new ScanTargetDto
            {
                Id = scanTarget.Id,
                Path = scanTarget.Path,
                IsScanned = scanTarget.IsScanned,
                IsInvalid = scanTarget.IsInvalid,
            };
        }
    }
}