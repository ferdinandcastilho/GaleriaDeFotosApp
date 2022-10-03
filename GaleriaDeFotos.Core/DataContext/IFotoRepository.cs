using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.Core.DataContext;

public interface IFotoRepository
{
    Task<FotoData> Insert(FotoData fotoData);
    Task<FotoData> Update(FotoData fotoData);
    Task<bool> Delete(FotoData fotoData);
    Task ClearAll();
    Task<List<FotoData>> GetAllFotos();
}