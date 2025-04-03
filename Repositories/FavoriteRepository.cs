using FoodWebMVC.Interfaces;
using FoodWebMVC.Models;

namespace FoodWebMVC.Repositories;

public class FavoriteRepository : RepositoryBase<Favorite>, IFavoriteRepository
{
	public FavoriteRepository(FoodWebMVCDbContext context) : base(context)
	{
	}
}