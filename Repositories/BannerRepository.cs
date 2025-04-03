using FoodWebMVC.Interfaces;
using FoodWebMVC.Models;

namespace FoodWebMVC.Repositories;

public class BannerRepository : RepositoryBase<Banner>, IBannerRepository
{
	public BannerRepository(FoodWebMVCDbContext context) : base(context)
	{
	}
}