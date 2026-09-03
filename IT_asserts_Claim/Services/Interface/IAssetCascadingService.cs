using static ITAssetManagement.Dtos.AssetCascading;

namespace ITAssetManagement.Services.Interface;

public interface IAssetCascadingService
{
    Task<AssetCascadingDropdownDto> GetCascadingAsync();
}
