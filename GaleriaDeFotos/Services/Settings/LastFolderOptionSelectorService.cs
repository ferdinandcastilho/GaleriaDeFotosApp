using GaleriaDeFotos.Contracts.Abstractions;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.EnumTypes;

namespace GaleriaDeFotos.Services.Settings;

public class LastFolderOptionSelectorService : SettingSelectorService<LastFolderOption>
{
    protected override string SettingKey => nameof(LastFolderOption);

    public LastFolderOptionSelectorService(ILocalSettingsService localSettingsService) : base(
        localSettingsService)
    {
    }
}