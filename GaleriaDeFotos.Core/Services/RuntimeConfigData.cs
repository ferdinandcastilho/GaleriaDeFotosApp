using JetBrains.Annotations;

namespace GaleriaDeFotos.Core.Services;

/// <summary>
///     Config de Runtime
/// </summary>
public static class RuntimeConfigData
{
    /// <summary>
    ///     Está Empacotado?
    /// </summary>
    public static bool IsMsix { [UsedImplicitly] get; set; }

    /// <summary>
    ///     Pasta do Aplicativo
    /// </summary>
    public static string ApplicationFolder { get; set; }
}