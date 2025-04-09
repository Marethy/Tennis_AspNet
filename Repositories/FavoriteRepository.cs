﻿namespace Tennis.Repositories;

using Tennis.Interfaces;
using Tennis.Models;

public class FavoriteRepository(TennisWebMVCContext context)
    : RepositoryBase<Favorite>(context), IFavoriteRepository
{
}
