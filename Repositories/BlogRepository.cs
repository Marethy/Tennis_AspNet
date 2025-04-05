using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Repositories;

public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
{
	public BlogRepository(FoodWebMVCDbContext context) : base(context)
	{
	}
}