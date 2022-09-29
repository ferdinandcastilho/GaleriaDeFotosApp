﻿using Windows.Storage;
using GaleriaDeFotos.Contracts.Services;
using GaleriaDeFotos.Core.Contracts.Services;
using GaleriaDeFotos.Core.Helpers;
using GaleriaDeFotos.Helpers;
using GaleriaDeFotos.Models;
using Microsoft.Extensions.Options;

namespace GaleriaDeFotos.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private const string DefaultApplicationDataFolder = "GaleriaDeFotos/ApplicationData";
    private const string DefaultLocalSettingsFile = "LocalSettings.json";
    public const string LastFolderKey = nameof(LastFolderKey);
    private readonly string _applicationDataFolder;

    private readonly IFileService _fileService;

    private readonly string _localApplicationData =
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    private readonly string _localSettingsFile;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly LocalSettingsOptions _options;

    private bool _isInitialized;

    private IDictionary<string, object> _settings;

    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;
        _options = options.Value;

        _applicationDataFolder = Path.Combine(_localApplicationData,
            _options.ApplicationDataFolder ?? DefaultApplicationDataFolder);
        _localSettingsFile = _options.LocalSettingsFile ?? DefaultLocalSettingsFile;

        _settings = new Dictionary<string, object>();
    }

    #region ILocalSettingsService Members

    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        if (RuntimeHelper.IsMsix)
        {
            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj))
                return await Json.ToObjectAsync<T>((string)obj);
        } else
        {
            await InitializeAsync();

            if (_settings.TryGetValue(key, out var obj))
                return await Json.ToObjectAsync<T>((string)obj);
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        if (RuntimeHelper.IsMsix)
        {
            ApplicationData.Current.LocalSettings.Values[key] = await Json.StringifyAsync(value);
        } else
        {
            await InitializeAsync();

            _settings[key] = await Json.StringifyAsync(value);

            await Task.Run(() =>
                _fileService.Save(_applicationDataFolder, _localSettingsFile, _settings));
        }
    }

    #endregion

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _settings =
                await Task.Run(() =>
                    _fileService.Read<IDictionary<string, object>>(_applicationDataFolder,
                        _localSettingsFile)) ?? new Dictionary<string, object>();

            _isInitialized = true;
        }
    }
}