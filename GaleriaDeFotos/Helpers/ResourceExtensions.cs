﻿using Microsoft.Windows.ApplicationModel.Resources;

namespace GaleriaDeFotos.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceLoader _resourceLoader = new();

    public static string GetLocalized(this string resourceKey)
    {
        return _resourceLoader.GetString(resourceKey);
    }
}