namespace Tennis.Repositories;

using Tennis.Data;
using Tennis.Interfaces;
using Tennis.Models;

public class BlogRepository(TennisWebMVCContext context)
    : RepositoryBase<Blog>(context), IBlogRepository
{
}
