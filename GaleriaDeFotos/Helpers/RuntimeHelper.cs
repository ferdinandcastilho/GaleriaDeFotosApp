using System.Runtime.InteropServices;
using System.Text;

namespace GaleriaDeFotos.Helpers;

/// <summary>
///     Acessórios sobre informações de Tempo de Execução
/// </summary>
public static class RuntimeHelper
{
    /// <summary>
    ///     Retorna se a aplicação é um Pacote do Windows
    /// </summary>
    public static bool IsMsix
    {
        get
        {
            var length = 0;

            return GetCurrentPackageFullName(ref length, null) != 15700L;
        }
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetCurrentPackageFullName(ref int packageFullNameLength,
        StringBuilder? packageFullName);
}