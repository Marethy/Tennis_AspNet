using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Repositories;

public class BannerRepository(TennisWebMVCContext context)
    : RepositoryBase<Banner>(context), IBannerRepository
{
}
