namespace Tennis.Repositories;

using Tennis.Interfaces;
using Tennis.Models;

public class BlogRepository(TennisWebMVCContext context)
    : RepositoryBase<Blog>(context), IBlogRepository
{
}
