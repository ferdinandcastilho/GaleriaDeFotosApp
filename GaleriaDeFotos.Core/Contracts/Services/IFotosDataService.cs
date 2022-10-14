﻿using System.Linq.Expressions;
using GaleriaDeFotos.Core.Models;

namespace GaleriaDeFotos.Core.Contracts.Services;

// Remove this class once your pages/features are using your data.
public interface IFotosDataService
{
    Task<IEnumerable<Foto>> GetPhotosAsync(string imagePath);
    Task<IEnumerable<Foto>> SelectAsync(Expression<Func<FotoData, bool>> predicate);
    void SetFavorite(Foto foto, bool isFavorite);
}