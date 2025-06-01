namespace Tennis.Repositories;

using Tennis.Data;
using Tennis.Interfaces;
using Tennis.Models;

public class FavoriteRepository(TennisWebMVCContext context)
    : RepositoryBase<Favorite>(context), IFavoriteRepository
{
}
