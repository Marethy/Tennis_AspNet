using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Repositories;

public class OrderDetailRepository(TennisWebMVCContext context)
    : RepositoryBase<OrderDetail>(context), IOrderDetailRepository
{
}
