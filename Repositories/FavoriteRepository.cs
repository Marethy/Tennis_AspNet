using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Repositories;

public class FavoriteRepository : RepositoryBase<Favorite>, IFavoriteRepository
{
	public FavoriteRepository(FoodWebMVCDbContext context) : base(context)
	{
	}
}