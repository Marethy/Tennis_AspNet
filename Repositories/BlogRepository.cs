using FoodWebMVC.Interfaces;
using FoodWebMVC.Models;

namespace FoodWebMVC.Repositories;

public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
{
	public BlogRepository(FoodWebMVCDbContext context) : base(context)
	{
	}
}