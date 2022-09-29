using GaleriaDeFotos.Contracts.Abstractions;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.EnumTypes;

namespace GaleriaDeFotos.Services.Settings;

public class LastFolderOptionSelectorService : SettingSelectorService<LastFolderOption>
{
    public LastFolderOptionSelectorService(ILocalSettingsService localSettingsService) : base(
        localSettingsService)
    {
    }

    protected override string SettingKey => nameof(LastFolderOption);
}