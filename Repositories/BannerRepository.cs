using Tennis.Interfaces;
using Tennis.Models;
using Tennis.Interfaces;

namespace Tennis.Repositories;

public class BannerRepository : RepositoryBase<Banner>, IBannerRepository
{
	public BannerRepository(FoodWebMVCDbContext context) : base(context)
	{
	}
}