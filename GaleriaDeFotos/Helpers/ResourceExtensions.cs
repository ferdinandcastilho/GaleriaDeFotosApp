﻿using Microsoft.Windows.ApplicationModel.Resources;

namespace GaleriaDeFotos.Helpers;

public static class ResourceExtensions
{
    private static readonly ResourceLoader ResourceLoader = new();

    public static string GetLocalized(this string resourceKey)
    {
        return ResourceLoader.GetString(resourceKey);
    }
}