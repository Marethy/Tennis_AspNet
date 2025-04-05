using Tennis.Interfaces;
using Tennis.Models;

namespace Tennis.Repositories;

public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
{
	public OrderDetailRepository(FoodWebMVCDbContext context) : base(context)
	{
	}
}