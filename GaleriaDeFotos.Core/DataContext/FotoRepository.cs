using GaleriaDeFotos.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GaleriaDeFotos.Core.DataContext;

public class FotoRepository : IFotoRepository
{
    private readonly FotoContext _fotoContext;

    public FotoRepository(FotoContext fotoContext) { _fotoContext = fotoContext; }

    #region IFotoRepository Members

    public async Task<FotoData> Insert(FotoData fotoData)
    {
        await _fotoContext.AddAsync(fotoData);

        await _fotoContext.SaveChangesAsync();

        return fotoData;
    }

    public async Task<FotoData> Update(FotoData fotoData)
    {
        var recordToUpdate = await _fotoContext.Fotos.FindAsync(fotoData);

        await _fotoContext.SaveChangesAsync();

        return recordToUpdate;
    }

    public async Task<bool> Delete(FotoData fotoData)
    {
        _fotoContext.Fotos.Remove(fotoData);

        await _fotoContext.SaveChangesAsync();

        return await _fotoContext.Fotos.FindAsync(fotoData) is null;
    }

    public async Task ClearAll()
    {
        var list = await _fotoContext.Fotos.ToListAsync();
        _fotoContext.Fotos.RemoveRange(list);

        await _fotoContext.SaveChangesAsync();
    }

    public async Task<List<FotoData>> GetAllFotos()
    {
        return await _fotoContext.Fotos.AsNoTracking().ToListAsync();
    }

    #endregion
}